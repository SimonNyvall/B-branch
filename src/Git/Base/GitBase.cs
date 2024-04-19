using System.Text;
using CliWrap;
using TableData;

namespace Git.Base;

public class GitBase
{
    private static GitBase? _instance;

    private string _gitPath = string.Empty;

    private GitBase()
    {
        SetGitPath();
    }

    public static GitBase GetInstance()
    {
        if (_instance is not null)
        {
            return _instance;
        }

        lock (typeof(GitBase))
        {
            _instance = _instance is null ? new GitBase() : _instance;
        }

        return _instance;
    }

    private void SetGitPath()
    {
        Execute execute = Execute.GetInstance();

        const string arguments = "rev-parse --git-dir";

        string stdOut = execute.ExecuteCommand(arguments);

        _gitPath = stdOut.Trim();
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

    public List<GitBranch> GetBranchNames(bool all, bool remote) { }

    private List<GitBranch> GetLocalBranchNames()
    {
        const string localBranchPath = Path.Combine(_gitPath, "refs", "heads");
    }

    private List<GitBranch> GetRemoteBranchNames()
    {
        const string remoteBranchPath = Path.Combine(_gitPath, "refs", "remotes");
    }

    protected static List<GitBranch> GetNamesAndLastWirte(bool all, bool remote)
    {
        Dictionary<string, DateTime> branches = [];

        string localBranchDir = Path.Combine(_gitPath, "refs", "heads");
        string remoteBranchDir = Path.Combine(_gitPath, "refs", "remotes");

        if (!remote)
        {
            CollectBranches(localBranchDir, ref branches);
        }

        if (all || remote)
        {
            CollectBranches(remoteBranchDir, ref branches);
        }

        IEnumerable<GitBranch> result = branches.Select(x => new GitBranch(x.Key, x.Value));

        result = result.OrderByDescending(x => x.LastCommit);

        return result.ToList();
    }

    private static void CollectBranches(string directory, ref Dictionary<string, DateTime> branches)
    {
        if (!Directory.Exists(directory))
        {
            throw new DirectoryNotFoundException($"Branch directory does not exist at {directory}");
        }

        foreach (string file in Directory.GetFiles(directory, "*", SearchOption.AllDirectories))
        {
            string relativePath = Path.GetRelativePath(directory, file);
            string branchName = relativePath.Replace(Path.DirectorySeparatorChar, '/');
            branches[branchName] = File.GetLastWriteTime(file);
        }
    }

    public static async Task<string> GetBranchDescription(string branchName)
    {
        if (!File.Exists(Path.Combine(_gitPath, "EDIT_DESCRIPTION")))
        {
            return string.Empty;
        }

        string descriptionFile = await File.ReadAllTextAsync(
            Path.Combine(_gitPath, "EDIT_DESCRIPTION")
        );

        if (!descriptionFile.Contains(branchName))
        {
            return string.Empty;
        }

        string[] lines = descriptionFile.Split('\n');

        IEnumerable<string> linesWithoutComments = lines.Where(x => !x.StartsWith('#'));

        string description = string.Join(" ", linesWithoutComments);

        return description;
    }

    protected static async Task<bool> ExecuteGitCommand(string gitPath, string arguments)
    {
        CommandResult result = await Cli.Wrap("git")
            .WithWorkingDirectory(gitPath)
            .WithArguments(arguments)
            .WithValidation(CommandResultValidation.None)
            .ExecuteAsync();

        return result.ExitCode == 0;
    }

    protected static string? GetOption(Dictionary<string, string> options, params string[] keys)
    {
        foreach (string key in keys)
        {
            bool found = options.TryGetValue(key, out string? value);

            if (found)
            {
                return value;
            }
        }

        return null;
    }

    protected static void LogError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
    }
}
