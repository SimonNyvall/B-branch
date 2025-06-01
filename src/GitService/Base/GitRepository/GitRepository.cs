using Bbranch.Shared.TableData;
using Bbranch.GitService.Base.Commands;

namespace Bbranch.GitService.Base;

public sealed class GitRepository : IGitRepository
{
    public string GitRepositoryPath { get => _gitPath; }
    
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
