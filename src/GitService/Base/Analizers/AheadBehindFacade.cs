using System.Buffers;
using System.Globalization;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;
using Bbranch.GitService.Base.Commands;
using Bbranch.Shared.TableData;

namespace Git_Service.Base.Analizers;

public class AheadBehindFacade(string gitPath)
{
    private const int MaxCacheSize = 1000;
    private const int DecompressionBufferSize = 4096;

    private readonly Dictionary<string, string> _commitParentCache = new(MaxCacheSize);
    private readonly Dictionary<string, byte[]> _objectCache = new(MaxCacheSize);

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
        string localBranchRefPath = Path.Combine(gitPath, "refs", "heads", localBranchName);
        string remoteBranchRefPath = remoteBranchName is null
            ? Path.Combine(gitPath, "refs", "remotes", "origin", localBranchName)
            : Path.Combine(gitPath, "refs", "remotes", "origin", remoteBranchName);

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

            string commitObjectPath = Path.Combine(gitPath, "objects", dirName, fileName);

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
            _objectCache[hash] = compressedData;
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
                var hashBytes = line.Slice(parentPrefix.Length).TrimStart((byte)' ');
                var parentHash = Encoding.UTF8.GetString(hashBytes);
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
}
