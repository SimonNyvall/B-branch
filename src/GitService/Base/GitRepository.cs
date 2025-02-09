using System.Globalization;
using System.Text.RegularExpressions;
using Bbranch.Shared.TableData;
using Bbranch.GitService.Base.Commands;
using System.IO.Compression;
using System.Text;

namespace Bbranch.GitService.Base;

public class GitRepository : IGitRepository
{
    private static GitRepository? _instance;

    private string _gitPath = string.Empty;

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
        GitRepositoryCheckCommand isGitRepositoryCommand = new();

        if (!isGitRepositoryCommand.Execute())
        {
            Console.WriteLine("fatal: not a git repository (or any parent up to mount point /)");
            Environment.Exit(1);
        }

        GitRepositoryLocationCommand gitDirectoryLocationCommand = new();

        _gitPath = gitDirectoryLocationCommand.Execute();
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

            DateTime lastWriteTimeOfCommit = File.GetLastWriteTime(Path.Combine(_gitPath, "objects", dirName, commitHash));

            return lastWriteTimeOfCommit;
        }
        catch
        {
            LastCommitDateFetchCommand lastCommitDateCommand = new(branchName);

            return lastCommitDateCommand.Execute();
        }
    }

    public AheadBehind GetLocalAheadBehind(string localBranchName)
    {
        try
        {
            string localBranchRefPath = Path.Combine(_gitPath, "refs", "heads", localBranchName);
            string remoteBranchRefPath = Path.Combine(_gitPath, "refs", "remotes", "origin", localBranchName);

            if (!File.Exists(remoteBranchRefPath))
            {
                return new(0, 0);
            }

            string localCommitHash = File.ReadAllText(localBranchRefPath).Trim();
            string remoteCommitHash = File.ReadAllText(remoteBranchRefPath).Trim();

            int ahead = CountCommitsBetween(localCommitHash, remoteCommitHash, direction: "ahead");
            int behind = CountCommitsBetween(localCommitHash, remoteCommitHash, direction: "behind");

            return new(ahead, behind);
        }
        catch
        {
            DefaultAheadBehindStatusCommand defaultAheadBehindCommand = new(localBranchName);

            return ParseAheadBehind(defaultAheadBehindCommand.Execute());
        }
    }

    private int CountCommitsBetween(string startHash, string endHash, string direction)
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

            string parentCommitHash = GetParentCommitHash(commitObjectPath);

            count++;
            currentHash = parentCommitHash;

            if (string.IsNullOrEmpty(currentHash))
            {
                break;
            }
        }

        return count;
    }

    private static string GetParentCommitHash(string commitObjectPath)
    {
        byte[] compressedData = File.ReadAllBytes(commitObjectPath);
        string deCompressedData = DecompressGitObject(compressedData);

        using StringReader reader = new(deCompressedData);

        string line;
        while ((line = reader.ReadLine()!) is not null)
        {
            if (line.StartsWith("parent "))
            {
                return line[7..].Trim();
            }
        }

        return string.Empty;
    }

    private static string DecompressGitObject(byte[] compressedData)
    {
        using MemoryStream compressedStream = new(compressedData);
        using ZLibStream zLibStream = new(compressedStream, CompressionMode.Decompress);
        using MemoryStream deCompressedStream = new();

        zLibStream.CopyTo(deCompressedStream);
        byte[] deCompressedData = deCompressedStream.ToArray();

        return Encoding.UTF8.GetString(deCompressedData);
    }

    public AheadBehind GetRemoteAheadBehind(string localBranchName, string remoteBranchName)
    {
        TrackAheadBehindStatusCommand trackAheadBehindCommand = new(localBranchName, remoteBranchName);

        return ParseAheadBehind(trackAheadBehindCommand.Execute());
    }

    private static AheadBehind ParseAheadBehind(string result)
    {
        if (string.IsNullOrWhiteSpace(result))
        {
            return new AheadBehind(0, 0);
        }

        Match match = Regex.Match(result, @"(\d+)\s+(\d+)", RegexOptions.Compiled);

        if (match.Success)
        {
            int ahead = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
            int behind = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);

            return new AheadBehind(ahead, behind);
        }

        return new AheadBehind(0, 0);
    }

    public List<GitBranch> GetLocalBranchNames()
    {
        string localBranchPath = Path.Combine(_gitPath, "refs", "heads");

        List<GitBranch> updatedBranches = CollectBrancheNames(localBranchPath);

        return updatedBranches;
    }

    public List<GitBranch> GetRemoteBranchNames()
    {
        string remoteBranchPath = Path.Combine(_gitPath, "refs", "remotes");

        List<GitBranch> updatedBranches = CollectBrancheNames(remoteBranchPath);
        return updatedBranches;
    }

    private static List<GitBranch> CollectBrancheNames(string directoryPath)
    {
        List<GitBranch> branches = [];
        var files = Directory.GetFiles(directoryPath, "*", SearchOption.AllDirectories);

        foreach (string file in files)
        {
            string relativePath = Path.GetRelativePath(directoryPath, file);
            string branchName = relativePath.Replace(Path.DirectorySeparatorChar, '/');

            Branch branch = new() { Name = branchName, IsWorkingBranch = false };

            branches.Add(GitBranch.Default().SetBranch(branch));
        }

        return branches;
    }

    public List<GitBranch> GetBranchDescription(List<GitBranch> branches)
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

    public bool DoesBranchExist(string branchName)
    {
        BranchExistenceCheckCommand doesBranchExistCommand = new(branchName);

        return doesBranchExistCommand.Execute();
    }
}
