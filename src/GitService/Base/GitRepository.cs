using System.Globalization;
using System.Text.RegularExpressions;
using Bbranch.Shared.TableData;
using Bbranch.GitService.Base.Commands;
using System.IO.Compression;
using System.Text;
using System.Buffers;

namespace Bbranch.GitService.Base;

public sealed class GitRepository : IGitRepository
{
    private const int MaxCacheSize = 1000;
    private const int DecompressionBufferSize = 4096;
    private static GitRepository? _instance;

    private string _gitPath = string.Empty;

    private readonly Dictionary<string, string> _commitParentCache = new(MaxCacheSize);
    private readonly Dictionary<string, byte[]> _objectCache = new(MaxCacheSize);

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
        var currentDirectory = Directory.GetCurrentDirectory();

        while (!string.IsNullOrEmpty(currentDirectory))
        {
            var gitPath = Path.Combine(currentDirectory, ".git");

            if (Directory.Exists(gitPath))
            {
                _gitPath = gitPath;
                return;
            }

            if (File.Exists(gitPath))
            {
                var gitFileContent = File.ReadAllText(gitPath).Trim();
                if (gitFileContent.StartsWith("gitdir:"))
                {
                    _gitPath = Path.GetFullPath(gitFileContent[7..].Trim(), currentDirectory);
                    return;
                }
            }

            currentDirectory = Directory.GetParent(currentDirectory)?.FullName;
        }

        Console.WriteLine("fatal: not a git repository (or any parent up to mount point /)");
        Environment.Exit(1);
    }

    public string GetWorkingBranch()
    {
        string headFilePath = Path.Combine(_gitPath, "HEAD");
        string headFileContent = File.ReadAllText(headFilePath).Trim();

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
            string commitHash = File.ReadAllText(Path.Combine(_gitPath, "refs", "heads", branchName)).Trim();

            string dirName = commitHash[..2];
            commitHash = commitHash[2..];

            if (!Directory.Exists(Path.Combine(_gitPath, "objects", dirName)))
            {
                throw new DirectoryNotFoundException();
            }

            DateTime lastWriteTimeOfCommit =
                File.GetLastWriteTime(Path.Combine(_gitPath, "objects", dirName, commitHash));

            if (lastWriteTimeOfCommit.Year <=
                1601) // File.GetLastWriteTime returns 1601-01-01 if the file does not exist
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

    public async Task<AheadBehind> GetRemoteAheadBehind(string localBranchName, string remoteBranchName)
    {
        try
        {
            return await GetAheadBehind(localBranchName, remoteBranchName);
        }
        catch (Exception)
        {
            TrackAheadBehindStatusCommand trackAheadBehindCommand = new(localBranchName, remoteBranchName);

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

    private async Task<AheadBehind> GetAheadBehind(string localBranchName, string? remoteBranchName = null)
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
            if (lineEnd == -1) break;
            
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
        return FetchBranches(localBranchPath);
    }

    public HashSet<GitBranch> GetRemoteBranchNames()
    {
        var remoteBranchPath = Path.Combine("refs", "remotes");
        return FetchBranches(remoteBranchPath);
    }

    private HashSet<GitBranch> FetchBranches(string path)
    {
        var updatedBranches = CollectBranchNames(path);

        path = path.Replace('\\', '/');
        updatedBranches = GetMergedBranchList(updatedBranches, GetPackedRefsBranches(path));

        return updatedBranches;
    }

    private HashSet<GitBranch> GetMergedBranchList(HashSet<GitBranch> headBranches, HashSet<GitBranch> refBranches)
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

        if (!File.Exists(packedRefsPath)) return [];

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
        var files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);

        foreach (var file in files)
        {
            var relativePath = Path.GetRelativePath(path, file);
            var branchName = relativePath.Replace(Path.DirectorySeparatorChar, '/');

            Branch branch = new(branchName, false);

            branches.Add(GitBranch.Default().SetBranch(branch));
        }

        return branches;
    }

    public HashSet<GitBranch> GetBranchDescription(HashSet<GitBranch> branches)
    {
        const string descriptionFileName = "EDIT_DESCRIPTION";

        if (!File.Exists(Path.Combine(_gitPath, descriptionFileName)))
        {
            return branches;
        }

        string descriptionFile = File.ReadAllText(Path.Combine(_gitPath, descriptionFileName));

        foreach (GitBranch branch in branches)
        {
            if (!descriptionFile.Contains(branch.Branch.Name))
            {
                continue;
            }

            IEnumerable<string> lines = descriptionFile.Split('\n');
            lines = lines.Where(line => !line.StartsWith('#'));
            string description = string.Join(" ", lines);
            branch.SetDescription(description);
        }

        return branches;
    }
}
