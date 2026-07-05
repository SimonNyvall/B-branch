using System.Buffers;
using System.IO.Compression;
using System.Text;
using Bbranch.GitService.Base.Commands;
using Bbranch.Shared.TableData;
using LibGit2Sharp;

namespace Bbranch.GitService.Base;

public sealed class GitRepository : IGitRepository
{
    private const int MaxCacheSize = 1000;
    private const int DecompressionBufferSize = 4096;
    internal static GitRepository? _instance;
    internal static IIOAbstration _iOAbstration = null!;

    internal static string _gitPath = string.Empty;
    internal static bool _isWorktreeRepo = false;

    private readonly Dictionary<string, IReadOnlyList<string>> _commitParentCache = new(
        MaxCacheSize
    );
    private readonly Dictionary<string, byte[]> _objectCache = new(MaxCacheSize);

    private string _headContent = string.Empty;

    private GitRepository(IIOAbstration? iOAbstration)
    {
        iOAbstration ??= new IOAbstraction();
        _iOAbstration = iOAbstration;
    }

    public static async Task<GitRepository> GetInstance(IIOAbstration? iOAbstration = null)
    {
        _instance ??= new GitRepository(iOAbstration);
        await SetGitPath();

        return _instance;
    }

    public static GitRepository GetInstanceForTests(IIOAbstration iOAbstration)
    {
        _instance ??= new GitRepository(iOAbstration);

        return _instance;
    }

    private static async Task SetGitPath()
    {
        var dir = _iOAbstration.GetCurrentDirectory();

        while (dir != null)
        {
            var dotGit = Path.Combine(dir.FullName, ".git");

            if (_iOAbstration.DirectoryExists(dotGit))
            {
                _gitPath = dotGit;
                return;
            }

            if (_iOAbstration.FileExists(dotGit))
            {
                var content = (await _iOAbstration.ReadAllText(dotGit)).Trim();

                if (content.StartsWith("gitdir:", StringComparison.OrdinalIgnoreCase))
                {
                    var path = content.Substring(7).Trim();
                    _gitPath = Path.GetFullPath(path, dir.FullName);
                    return;
                }
            }

            if (LooksLikeGitDir(dir.FullName))
            {
                _gitPath = dir.FullName;
                _isWorktreeRepo = true;
                return;
            }

            dir = dir.Parent;
        }

        Console.WriteLine("fatal: not a git repository (or any parent up to mount point /)");
        Environment.Exit(1);
    }

    private static bool LooksLikeGitDir(string path)
    {
        return _iOAbstration.FileExists(Path.Combine(path, "HEAD"))
            && _iOAbstration.DirectoryExists(Path.Combine(path, "objects"))
            && _iOAbstration.DirectoryExists(Path.Combine(path, "refs"));
    }

    public async Task<string> GetWorkingBranch()
    {
        string headFileContent = string.Empty;

        if (string.IsNullOrEmpty(_headContent))
        {
            string headFilePath = Path.Combine(_gitPath, "HEAD");
            headFileContent = (await _iOAbstration.ReadAllText(headFilePath)).Trim();
        }
        else
        {
            headFileContent = _headContent;
        }

        if (headFileContent.StartsWith("ref:", StringComparison.Ordinal))
        {
            const int branchNameIndex = 2;
            string[] branchNameComponents = headFileContent.Split('/');

            string currentWorkingBranchName = string.Join(
                "/",
                branchNameComponents.Skip(branchNameIndex)
            );
            return currentWorkingBranchName;
        }

        return string.Empty;
    }

    public async Task<GitBranch> GetLastCommitDate(GitBranch branch)
    {
        var commitPath = branch.IsRemote ? "remotes" : "heads";
        var refPath = Path.Combine(_gitPath, "refs", commitPath, branch.Branch.Name);

        if (!_iOAbstration.FileExists(refPath))
        {
            return RunLastCommitDataFetchCommand(branch);
        }

        string commitHash = (await _iOAbstration.ReadAllText(refPath)).Trim();

        if (string.IsNullOrWhiteSpace(commitHash) || commitHash.Length < 3)
        {
            return RunLastCommitDataFetchCommand(branch);
        }

        string dirName = commitHash[..2];
        string objectName = commitHash[2..];

        var objectDir = Path.Combine(_gitPath, "objects", dirName);

        if (!_iOAbstration.DirectoryExists(objectDir))
        {
            return RunLastCommitDataFetchCommand(branch);
        }

        var objectPath = Path.Combine(objectDir, objectName);

        if (!_iOAbstration.FileExists(objectPath))
        {
            return RunLastCommitDataFetchCommand(branch);
        }

        DateTime lastWriteTime = _iOAbstration.GetLastWriteTime(objectPath);

        if (lastWriteTime.Year <= 1601)
        {
            return RunLastCommitDataFetchCommand(branch);
        }

        return branch.SetLastCommit(lastWriteTime);
    }

    private GitBranch RunLastCommitDataFetchCommand(GitBranch branch)
    {
        var branchName = branch.IsSymbolic ? branch.Branch.Name.Split(' ')[^1] : branch.Branch.Name;

        var lastWriteTime = new LastCommitDateFetchCommand(branchName).Execute();
        return branch.SetLastCommit(lastWriteTime);
    }

    public async Task<GitBranch> GetRemoteAheadBehind(
        GitBranch localBranch,
        string remoteBranchName
    )
    {
        try
        {
            return await GetAheadBehind(localBranch, remoteBranchName);
        }
        catch (Exception)
        {
            return localBranch.SetAheadBehind(new AheadBehind(0, 0));
        }
    }

    public async Task<GitBranch> GetLocalAheadBehind(GitBranch localBranch)
    {
        try
        {
            return await GetAheadBehind(localBranch);
        }
        catch
        {
            return localBranch.SetAheadBehind(new AheadBehind(0, 0));
        }
    }

    internal async Task<GitBranch> GetAheadBehind(
        GitBranch localGitBranch,
        string? remoteBranchName = null
    )
    {
        using var repo = new Repository(_gitPath);

        var local = repo.Branches[localGitBranch.Branch.Name];

        var remote = remoteBranchName is null
            ? repo.Branches[$"origin/{localGitBranch.Branch.Name}"]
            : repo.Branches[$"origin/{remoteBranchName}"];

        if (local is null || remote is null)
        {
            return localGitBranch.SetAheadBehind(new AheadBehind(0, 0));
        }

        var divergence = repo.ObjectDatabase.CalculateHistoryDivergence(local.Tip, remote.Tip);

        var aheadBehind = new AheadBehind(divergence.AheadBy ?? 0, divergence.BehindBy ?? 0);
        return localGitBranch.SetAheadBehind(aheadBehind);
    }

    private async Task<IReadOnlyList<string>> GetParentCommitHashes(string commitObjectPath)
    {
        string hash = GetHashFromPath(commitObjectPath);

        if (_commitParentCache.TryGetValue(hash, out var cached))
        {
            return cached;
        }

        byte[] compressedData;
        if (!_objectCache.TryGetValue(hash, out compressedData!))
        {
            compressedData = await _iOAbstration.ReadAllBytes(commitObjectPath);
            _objectCache[hash] = compressedData;
        }

        byte[] decompressed = await DecompressGitObject(compressedData);

        List<string> parents = [];

        using var reader = new StringReader(Encoding.UTF8.GetString(decompressed));

        while (reader.ReadLine() is { } line)
        {
            if (line.StartsWith("parent "))
            {
                parents.Add(line["parent ".Length..]);
            }
        }

        _commitParentCache[hash] = parents;

        return parents;
    }

    private static string GetHashFromPath(string path)
    {
        // TODO: add this into the IOAbstraction
        var file = Path.GetFileName(path);
        var dir = Path.GetFileName(Path.GetDirectoryName(path)!);

        return dir + file;
    }

    private async Task<HashSet<string>> GetReachableCommits(string startHash)
    {
        HashSet<string> visited = [];
        Stack<string> stack = new();

        stack.Push(startHash);

        while (stack.Count > 0)
        {
            var hash = stack.Pop();

            if (!visited.Add(hash))
                continue;

            string path = Path.Combine(_gitPath, "objects", hash[..2], hash[2..]);

            if (!_iOAbstration.FileExists(path))
                continue;

            var parents = await GetParentCommitHashes(path);

            foreach (var parent in parents)
            {
                stack.Push(parent);
            }
        }

        return visited;
    }

    private static async Task<byte[]> DecompressGitObject(ReadOnlyMemory<byte> compressedData)
    {
        using var compressedStream = new MemoryStream(compressedData.ToArray());
        await using var zLibStream = new ZLibStream(compressedStream, CompressionMode.Decompress);

        // Use ArrayPool for better memory usage
        byte[] buffer = ArrayPool<byte>.Shared.Rent(DecompressionBufferSize);
        try
        {
            using var decompressedStream = new MemoryStream();
            int read;
            while ((read = await zLibStream.ReadAsync(buffer)) > 0)
            {
                await decompressedStream.WriteAsync(buffer.AsMemory(0, read));
            }

            return decompressedStream.ToArray();
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }

    public async Task<List<GitBranch>> GetLocalBranchNames()
    {
        var localBranchPath = Path.Combine("refs", "heads");
        var localBranches = await FetchBranches(localBranchPath);

        var detachedHead = await GetDetachedHead();

        if (detachedHead != null)
        {
            localBranches.Add(detachedHead);
        }

        return localBranches;
    }

    public async Task<List<GitBranch>> GetRemoteBranchNames()
    {
        var remoteBranchPath = Path.Combine("refs", "remotes");

        var remoteBranches = await FetchBranches(remoteBranchPath);

        foreach (var remoteBranch in remoteBranches)
        {
            remoteBranch.SetIsRemote(true);
        }

        var remotesRoot = Path.Combine(_gitPath, "refs", "remotes");

        if (!_iOAbstration.DirectoryExists(remotesRoot))
        {
            return remoteBranches;
        }

        var remotes = _iOAbstration.GetDirectories(remotesRoot);

        foreach (var remoteDir in remotes)
        {
            var remoteName = _iOAbstration.GetFileName(remoteDir);
            var remoteHead = await GetRemoteHead(remoteName);

            if (remoteHead != null)
            {
                remoteBranches.Add(remoteHead);
            }
        }

        return remoteBranches;
    }

    private async Task<GitBranch?> GetRemoteHead(string remoteName)
    {
        var headPath = Path.Combine(_gitPath, "refs", "remotes", remoteName, "HEAD");

        if (!_iOAbstration.FileExists(headPath))
            return null;

        var content = await _iOAbstration.ReadAllText(headPath);

        if (!content.StartsWith("ref: "))
            return null;

        var target = content.Substring(5).Trim();
        var shortTarget = target.Replace("refs/remotes/", "");

        var displayName = $"{remoteName}/HEAD -> {shortTarget}";

        BranchViewModel branch = new(displayName, false);

        return GitBranch
            .Default()
            .SetBranch(branch)
            .SetIsRemote(true)
            .SetSymLink(new Symbolic(remoteName, shortTarget));
    }

    private async Task<GitBranch?> GetDetachedHead()
    {
        var headPath = Path.Combine(_gitPath, "HEAD");
        var headContent = await _iOAbstration.ReadAllText(headPath);
        _headContent = headContent;

        if (headContent.StartsWith("ref: "))
        {
            return null;
        }

        var commitHash = headContent[..7];

        var branchName = $"(HEAD detached at {commitHash})";

        var branch = new BranchViewModel(branchName, true);

        return GitBranch.Default().SetBranch(branch).SetDetachedHead(commitHash);
    }

    private async Task<List<GitBranch>> FetchBranches(string path)
    {
        var updatedBranches = CollectBranchNames(path);

        path = path.Replace('\\', '/');
        updatedBranches = GetMergedBranchList(updatedBranches, await GetPackedRefsBranches(path));

        return updatedBranches;
    }

    private List<GitBranch> GetMergedBranchList(
        List<GitBranch> headBranches,
        List<GitBranch> refBranches
    )
    {
        var branchNames = new List<string>(headBranches.Select(b => b.Branch.Name));

        foreach (var branch in refBranches)
        {
            if (!branchNames.Contains(branch.Branch.Name))
            {
                headBranches.Add(branch);
                branchNames.Add(branch.Branch.Name);
            }
        }

        return headBranches;
    }

    private async Task<List<GitBranch>> GetPackedRefsBranches(string prefix)
    {
        var packedRefsPath = Path.Combine(_gitPath, "packed-refs");

        if (!_iOAbstration.FileExists(packedRefsPath))
            return [];

        var branches = new List<GitBranch>();
        var packedRefsLines = await _iOAbstration.ReadAllLines(packedRefsPath);

        foreach (var line in packedRefsLines)
        {
            if (line.StartsWith('#') || string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            if (!line.Contains(prefix))
            {
                continue;
            }

            var parts = line.Split(' ');
            if (parts.Length < 2)
            {
                continue;
            }

            var branchName = parts[1];
            if (!branchName.StartsWith(prefix))
            {
                continue;
            }

            BranchViewModel branch = new(branchName.Replace($"{prefix}/", ""), false);

            branches.Add(GitBranch.Default().SetBranch(branch).SetIsPacked(true));
        }

        return branches;
    }

    private List<GitBranch> CollectBranchNames(string directoryPath)
    {
        List<GitBranch> branches = [];
        var path = Path.Combine(_gitPath, directoryPath);

        if (!_iOAbstration.DirectoryExists(path))
        {
            return branches;
        }

        var files = _iOAbstration.GetFiles(path);

        foreach (var file in files)
        {
            var relativePath = _iOAbstration.GetRelativePath(path, file);

            if (relativePath == null)
            {
                continue;
            }

            var branchName = relativePath.Replace(Path.DirectorySeparatorChar, '/');

            if (branchName.EndsWith("/HEAD"))
                continue;

            BranchViewModel branch = new(branchName, false);

            branches.Add(GitBranch.Default().SetBranch(branch));
        }

        return branches;
    }

    public async Task<List<GitBranch>> GetBranchDescription(List<GitBranch> branches)
    {
        const string descriptionFileName = "EDIT_DESCRIPTION";
        string path = Path.Combine(_gitPath, descriptionFileName);

        if (!_iOAbstration.FileExists(path))
            return branches;

        var lines = await _iOAbstration.ReadAllLines(path);

        var descriptions = new Dictionary<string, string>();
        string? currentBranch = null;
        var currentDescription = new List<string>();

        foreach (var rawLine in lines)
        {
            var line = rawLine.Trim();

            if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                continue;

            if (line.StartsWith("[") && line.EndsWith("]"))
            {
                if (currentBranch != null)
                {
                    descriptions[currentBranch] = string.Join(" ", currentDescription).Trim();
                }

                currentBranch = line[1..^1];
                currentDescription.Clear();
                continue;
            }

            if (currentBranch != null)
            {
                currentDescription.Add(line);
            }
        }

        if (currentBranch != null)
        {
            descriptions[currentBranch] = string.Join(" ", currentDescription).Trim();
        }

        foreach (var branch in branches)
        {
            if (descriptions.TryGetValue(branch.Branch.Name, out var desc))
            {
                branch.SetDescription(desc);
            }
        }

        return branches;
    }

    public List<GitBranch> StichWorkTreeBranches(List<GitBranch> branches)
    {
        if (!_isWorktreeRepo)
        {
            return branches;
        }

        var worktreePath = Path.Combine(_gitPath, "worktrees");

        var checkedOutBrancheNames = new List<string>();

        foreach (var worktreeDirectory in _iOAbstration.GetDirectories(worktreePath))
        {
            checkedOutBrancheNames.Add(worktreeDirectory.Split(Path.DirectorySeparatorChar)[^1]);
        }

        foreach (var branch in branches)
        {
            var worktreeBranch = checkedOutBrancheNames.FirstOrDefault(x =>
                branch.Branch.Name.Equals(x, StringComparison.CurrentCulture)
            );

            if (worktreeBranch == null)
            {
                continue;
            }

            branch.SetIsCheckoutWorktree(true);
        }

        return branches;
    }
}
