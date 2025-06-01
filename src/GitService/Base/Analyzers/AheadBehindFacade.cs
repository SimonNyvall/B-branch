using System.Collections.Concurrent;
using Bbranch.GitService.Base.Commands;
using Bbranch.Shared.TableData;
using System.IO.Compression;
using System.Text;

namespace Bbranch.GitService.Base.Analyzers;

public sealed class AheadBehindFacade
{
    private const int MaxCacheSize = 1000;
    private const int DecompressionBufferSize = 4096;
    private readonly string _gitPath;

    private readonly ConcurrentDictionary<string, string> _commitParentCache = new(Environment.ProcessorCount, MaxCacheSize);
    private readonly ConcurrentDictionary<string, byte[]> _objectCache = new(Environment.ProcessorCount, MaxCacheSize);

    public AheadBehindFacade(string gitPath)
    {
        _gitPath = gitPath;
    }

    public async Task<AheadBehind> GetLocalAheadBehind(string localBranchName)
    {
        try
        {
            return await GetAheadBehind(localBranchName);
        }
        catch (Exception)
        {
            var defaultAheadBehindCommand = new DefaultAheadBehindStatusCommand(localBranchName);
            return ParseAheadBehind(defaultAheadBehindCommand.Execute());
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
            var trackAheadBehindCommand = new TrackAheadBehindStatusCommand(localBranchName, remoteBranchName);
            return ParseAheadBehind(trackAheadBehindCommand.Execute());
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

        byte[] localContentBytes = await File.ReadAllBytesAsync(localBranchRefPath);
        byte[] remoteContentBytes = await File.ReadAllBytesAsync(remoteBranchRefPath);

        bool areEqual = localContentBytes.AsSpan().SequenceEqual(remoteContentBytes);
        if (areEqual)
        {
            return new(0, 0);
        }

        string localCommitHash = Encoding.UTF8.GetString(localContentBytes).Trim();
        string remoteCommitHash = Encoding.UTF8.GetString(remoteContentBytes).Trim();

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
            if (string.IsNullOrEmpty(parentCommitHash))
            {
                break;
            }

            count++;
            currentHash = parentCommitHash;
        }

        return count;
    }

    private async Task<string> GetParentCommitHash(string commitObjectPath)
    {
        string hash = Path.GetFileName(commitObjectPath);

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
            _objectCache.TryAdd(hash, compressedData);
        }

        byte[] decompressedBytes = await DecompressGitObject(compressedData);
        ReadOnlySpan<byte> decompressedSpan = decompressedBytes;
        ReadOnlySpan<byte> parentPrefix = "parent "u8;

        int pos = 0;
        while (pos < decompressedSpan.Length)
        {
            int lineEnd = decompressedSpan[pos..].IndexOf((byte)'\n');
            if (lineEnd == -1) break;
            
            var line = decompressedSpan.Slice(pos, lineEnd);
            
            if (line.StartsWith(parentPrefix))
            {
                string parentHash = Encoding.ASCII.GetString(line[parentPrefix.Length..]);
                _commitParentCache.TryAdd(hash, parentHash);
                return parentHash;
            }

            pos += lineEnd + 1;
        }

        return string.Empty;
    }

    private static async Task<byte[]> DecompressGitObject(ReadOnlyMemory<byte> compressedData)
    {
        using var compressedStream = new MemoryStream(compressedData.ToArray());
        using var zlibStream = new ZLibStream(compressedStream, CompressionMode.Decompress);
        using var decompressedStream = new MemoryStream();

        var buffer = new byte[DecompressionBufferSize];
        int bytesRead;

        while ((bytesRead = await zlibStream.ReadAsync(buffer)) > 0)
        {
            await decompressedStream.WriteAsync(buffer.AsMemory(0, bytesRead));
        }

        return decompressedStream.ToArray();
    }

    private static AheadBehind ParseAheadBehind(string result)
    {
        if (string.IsNullOrWhiteSpace(result))
            return new AheadBehind(0, 0);

        var numbers = result.Split('\t');
        if (numbers.Length == 2 && 
            int.TryParse(numbers[0], out int ahead) && 
            int.TryParse(numbers[1], out int behind))
        {
            return new AheadBehind(ahead, behind);
        }

        return new AheadBehind(0, 0);
    }
}
