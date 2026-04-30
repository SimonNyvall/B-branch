using System.Buffers;
using System.Globalization;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;
using Bbranch.GitService.Base.Commands;
using Bbranch.Shared.TableData;

namespace Bbranch.GitService.Base;

public sealed class GitRepository : IGitRepository
{
    private const int MaxCacheSize = 1000;
    private const int DecompressionBufferSize = 4096;
    private static GitRepository? _instance;

    private string _gitPath = string.Empty;
    private bool _isWorktreeRepo = false;

    private readonly Dictionary<string, string> _commitParentCache = new(MaxCacheSize);
    private readonly Dictionary<string, byte[]> _objectCache = new(MaxCacheSize);

    private string _headContent = string.Empty;

    private GitRepository()
    {
        SetGitPath();
    }

    public static GitRepository GetInstance()
    {
        if (_instance is not null)
        {
            return _instance;
        }

        lock (typeof(GitRepository))
        {
            _instance = _instance is null ? new GitRepository() : _instance;
        }

        return _instance;
    }

    private void SetGitPath()
    {
        var dir = new DirectoryInfo(Directory.GetCurrentDirectory());

        while (dir != null)
        {
            var dotGit = Path.Combine(dir.FullName, ".git");

            if (Directory.Exists(dotGit))
            {
                _gitPath = dotGit;
                return;
            }

            if (File.Exists(dotGit))
            {
                var content = File.ReadAllText(dotGit).Trim();

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
        return File.Exists(Path.Combine(path, "HEAD"))
            && Directory.Exists(Path.Combine(path, "objects"))
            && Directory.Exists(Path.Combine(path, "refs"));
    }

    public string GetWorkingBranch()
    {
        string headFileContent = string.Empty;

        if (string.IsNullOrEmpty(_headContent))
        {
            string headFilePath = Path.Combine(_gitPath, "HEAD");
            headFileContent = File.ReadAllText(headFilePath).Trim();
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

    public DateTime GetLastCommitDate(string branchName)
    {
        try
        {
            string commitHash = File.ReadAllText(
                    Path.Combine(_gitPath, "refs", "heads", branchName)
                )
                .Trim();

            string dirName = commitHash[..2];
            commitHash = commitHash[2..];

            if (!Directory.Exists(Path.Combine(_gitPath, "objects", dirName)))
            {
                throw new DirectoryNotFoundException();
            }

            DateTime lastWriteTimeOfCommit = File.GetLastWriteTime(
                Path.Combine(_gitPath, "objects", dirName, commitHash)
            );

            if (lastWriteTimeOfCommit.Year <= 1601) // File.GetLastWriteTime returns 1601-01-01 if the file does not exist
            {
                throw new InvalidDataException();
            }

            return lastWriteTimeOfCommit;
        }
        catch
        {
            LastCommitDateFetchCommand lastCommitDateCommand = new(branchName);

            return lastCommitDateCommand.Execute();
        }
    }

    public async Task<AheadBehind> GetRemoteAheadBehind(
        string localBranchName,
        string remoteBranchName
    )
    {
        try
        {
            return await GetAheadBehind(localBranchName, remoteBranchName);
        }
        catch (Exception)
        {
            TrackAheadBehindStatusCommand trackAheadBehindCommand = new(
                localBranchName,
                remoteBranchName
            );

            return ParseAheadBehind(trackAheadBehindCommand.Execute());
        }
    }

    public async Task<AheadBehind> GetLocalAheadBehind(string localBranchName)
    {
        try
        {
            return await GetAheadBehind(localBranchName);
        }
        catch
        {
            DefaultAheadBehindStatusCommand defaultAheadBehindCommand = new(localBranchName);

            return ParseAheadBehind(defaultAheadBehindCommand.Execute());
        }
    }

    private async Task<AheadBehind> GetAheadBehind(
        string localBranchName,
        string? remoteBranchName = null
    )
    {
        string localBranchRefPath = Path.Combine(_gitPath, "refs", "heads", localBranchName);
        string remoteBranchRefPath = remoteBranchName is null
            ? Path.Combine(_gitPath, "refs", "remotes", "origin", localBranchName)
            : Path.Combine(_gitPath, "refs", "remotes", "origin", remoteBranchName);

        if (!File.Exists(remoteBranchRefPath))
        {
            return new(0, 0);
        }

        // Read files as byte arrays
        byte[] localContentBytes = await File.ReadAllBytesAsync(localBranchRefPath);
        byte[] remoteContentBytes = await File.ReadAllBytesAsync(remoteBranchRefPath);

        // Compare contents
        bool areEqual = localContentBytes.AsSpan().SequenceEqual(remoteContentBytes);
        if (areEqual)
        {
            return new(0, 0);
        }

        // Convert to strings for commit traversal
        string localCommitHash = Encoding.UTF8.GetString(localContentBytes).Trim();
        string remoteCommitHash = Encoding.UTF8.GetString(remoteContentBytes).Trim();

        // Run ahead/behind counts in parallel
        var aheadTask = CountCommitsBetween(localCommitHash, remoteCommitHash, "ahead");
        var behindTask = CountCommitsBetween(localCommitHash, remoteCommitHash, "behind");

        await Task.WhenAll(aheadTask, behindTask);
        return new(aheadTask.Result, behindTask.Result);
    }

    private async Task<int> CountCommitsBetween(string startHash, string endHash, string direction)
    {
        int count = 0;
        string currentHash = direction == "ahead" ? startHash : endHash;
        string targetHash = direction == "ahead" ? endHash : startHash;

        while (currentHash != targetHash)
        {
            string dirName = currentHash[..2];
            string fileName = currentHash[2..];

            string commitObjectPath = Path.Combine(_gitPath, "objects", dirName, fileName);

            if (!File.Exists(commitObjectPath))
            {
                return 0;
            }

            string parentCommitHash = await GetParentCommitHash(commitObjectPath);

            count++;
            currentHash = parentCommitHash;

            if (string.IsNullOrEmpty(currentHash))
            {
                break;
            }
        }

        return count;
    }

    private async Task<string> GetParentCommitHash(string commitObjectPath)
    {
        // Extract the hash from the path and convert to string immediately
        string hash = Path.GetFileName(commitObjectPath);

        // Check cache using string
        if (_commitParentCache.TryGetValue(hash, out var cachedParent))
        {
            return cachedParent;
        }

        byte[] compressedData;
        if (_objectCache.TryGetValue(hash, out var cached))
        {
            compressedData = cached;
        }
        else
        {
            compressedData = await File.ReadAllBytesAsync(commitObjectPath);
            _objectCache[hash] = compressedData;
        }

        // Get decompressed data as byte array
        byte[] decompressedBytes = await DecompressGitObject(compressedData);
        ReadOnlySpan<byte> decompressedSpan = decompressedBytes;
        ReadOnlySpan<byte> parentPrefix = "parent "u8;

        int pos = 0;
        while (pos < decompressedSpan.Length)
        {
            // Find next line
            int lineEnd = decompressedSpan[pos..].IndexOf((byte)'\n');
            if (lineEnd == -1)
                break;

            var line = decompressedSpan.Slice(pos, lineEnd);

            if (line.StartsWith(parentPrefix))
            {
                // Extract parent hash without allocations
                var hashBytes = line.Slice(parentPrefix.Length).TrimStart((byte)' ');
                var parentHash = Encoding.UTF8.GetString(hashBytes); // Convert the 40-char hash
                _commitParentCache[hash] = parentHash;
                return parentHash;
            }

            pos += lineEnd + 1;
        }

        var emptyResult = string.Empty;
        _commitParentCache[hash] = emptyResult;
        return emptyResult;
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

    private static AheadBehind ParseAheadBehind(string result)
    {
        if (string.IsNullOrWhiteSpace(result))
        {
            return new AheadBehind(0, 0);
        }

        var match = Regex.Match(result, @"(\d+)\s+(\d+)", RegexOptions.Compiled);

        if (match.Success)
        {
            var ahead = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
            var behind = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);

            return new AheadBehind(ahead, behind);
        }

        return new AheadBehind(0, 0);
    }

    public HashSet<GitBranch> GetLocalBranchNames()
    {
        var localBranchPath = Path.Combine("refs", "heads");
        var localBranches = FetchBranches(localBranchPath);

        var detachedHead = GetDetachedHead();

        if (detachedHead != null)
        {
            localBranches.Add(detachedHead);
        }

        return localBranches;
    }

    public HashSet<GitBranch> GetRemoteBranchNames()
    {
        var remoteBranchPath = Path.Combine("refs", "remotes");

        var remoteBranches = FetchBranches(remoteBranchPath);

        foreach (var remoteBranch in remoteBranches)
        {
            remoteBranch.SetIsRemote(true);
        }

        var remotesRoot = Path.Combine(_gitPath, "refs", "remotes");

        if (!Directory.Exists(remotesRoot))
        {
            return remoteBranches;
        }

        var remotes = Directory.GetDirectories(remotesRoot);

        foreach (var remoteDir in remotes)
        {
            var remoteName = Path.GetFileName(remoteDir);
            var remoteHead = GetRemoteHead(remoteName);

            if (remoteHead != null)
            {
                remoteBranches.Add(remoteHead);
            }
        }

        return remoteBranches;
    }

    private GitBranch? GetRemoteHead(string remoteName)
    {
        var headPath = Path.Combine(_gitPath, "refs", "remotes", remoteName, "HEAD");

        if (!File.Exists(headPath))
            return null;

        var content = File.ReadAllText(headPath);

        if (!content.StartsWith("ref: "))
            return null;

        var target = content.Substring(5).Trim();
        var shortTarget = target.Replace("refs/remotes/", "");

        var displayName = $"{remoteName}/HEAD -> {shortTarget}";

        Branch branch = new(displayName, false);

        return GitBranch
            .Default()
            .SetBranch(branch)
            .SetIsRemote(true)
            .SetSymLink(new Symbolic(remoteName, shortTarget));
    }

    private GitBranch? GetDetachedHead()
    {
        var headPath = Path.Combine(_gitPath, "HEAD");
        var headContent = File.ReadAllText(headPath);
        _headContent = headContent;

        if (headContent.StartsWith("ref: "))
        {
            return null;
        }

        var commitHash = headContent[..7];

        var branchName = $"(HEAD detached at {commitHash})";

        var branch = new Branch(branchName, true);

        return GitBranch.Default().SetBranch(branch).SetDetachedHead(commitHash);
    }

    private HashSet<GitBranch> FetchBranches(string path)
    {
        var updatedBranches = CollectBranchNames(path);

        path = path.Replace('\\', '/');
        updatedBranches = GetMergedBranchList(updatedBranches, GetPackedRefsBranches(path));

        return updatedBranches;
    }

    private HashSet<GitBranch> GetMergedBranchList(
        HashSet<GitBranch> headBranches,
        HashSet<GitBranch> refBranches
    )
    {
        var branchNames = new HashSet<string>(headBranches.Select(b => b.Branch.Name));

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

    private HashSet<GitBranch> GetPackedRefsBranches(string prefix)
    {
        var packedRefsPath = Path.Combine(_gitPath, "packed-refs");

        if (!File.Exists(packedRefsPath))
            return [];

        var branches = new HashSet<GitBranch>();
        var packedRefsLines = File.ReadAllLines(packedRefsPath);

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

            Branch branch = new(branchName.Replace($"{prefix}/", ""), false);

            branches.Add(GitBranch.Default().SetBranch(branch));
        }

        return branches;
    }

    private HashSet<GitBranch> CollectBranchNames(string directoryPath)
    {
        HashSet<GitBranch> branches = [];
        var path = Path.Combine(_gitPath, directoryPath);

        if (!Directory.Exists(path))
        {
            return branches;
        }

        var files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);

        foreach (var file in files)
        {
            var relativePath = Path.GetRelativePath(path, file);
            var branchName = relativePath.Replace(Path.DirectorySeparatorChar, '/');

            if (branchName.EndsWith("/HEAD"))
                continue;

            Branch branch = new(branchName, false);

            branches.Add(GitBranch.Default().SetBranch(branch));
        }

        return branches;
    }

    public HashSet<GitBranch> GetBranchDescription(HashSet<GitBranch> branches)
    {
        const string descriptionFileName = "EDIT_DESCRIPTION";
        string path = Path.Combine(_gitPath, descriptionFileName);

        if (!File.Exists(path))
            return branches;

        var lines = File.ReadAllLines(path);

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

    public HashSet<GitBranch> StichWorkTreeBranches(HashSet<GitBranch> branches)
    {
        if (!_isWorktreeRepo)
        {
            return branches;
        }

        var worktreePath = Path.Combine(_gitPath, "worktrees");

        var checkedOutBrancheNames = new HashSet<string>();

        foreach (
            var worktreeDirectory in Directory.GetDirectories(
                worktreePath,
                "*",
                SearchOption.TopDirectoryOnly
            )
        )
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
