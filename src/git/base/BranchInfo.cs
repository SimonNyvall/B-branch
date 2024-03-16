namespace Bbranch.Git.Base.GitBase;

using System.Text;
using Bbranch.TableData;
using CliWrap;

public class GitBase
{
    protected static string gitPath = string.Empty;

    public static async Task<GitBase> Initialize()
    {
        await TrySetGitPath();

        return new GitBase();
    }

    public async Task<string> TryGetWorkingBranch()
    {
        try
        {
            return await GetWorkingBranch();
        }
        catch
        {
            return string.Empty;
        }
    }

    private async Task<string> GetWorkingBranch()
    {
        var HEADFile = await File.ReadAllTextAsync(Path.Combine(gitPath, "HEAD"));

        if (HEADFile.Trim().StartsWith("ref:"))
        {
            var branchNameComponents = HEADFile.Split('/');

            var branchName = string.Join("/", branchNameComponents.Skip(2));

            return branchName;
        }

        return string.Empty;
    }

    protected static List<GitBranch> GetNamesAndLastWirte(bool all, bool remote)
    {
        var branches = new Dictionary<string, DateTime>();

        var localBranchDir = Path.Combine(gitPath, "refs", "heads");
        var remoteBranchDir = Path.Combine(gitPath, "refs", "remotes");

        if (!remote)
        {
            CollectBranches(localBranchDir, ref branches);
        }

        if (all || remote)
        {
            CollectBranches(remoteBranchDir, ref branches);
        }

        IEnumerable<GitBranch> result = branches
            .Select(x => new GitBranch(x.Key, x.Value));

        result = result.OrderByDescending(x => x.LastCommit);

        return result.ToList();
    }

    private static void CollectBranches(string directory, ref Dictionary<string, DateTime> branches)
    {
        if (!Directory.Exists(directory)) throw new Exception($"Branch directory does not exist at {directory}");

        foreach (var file in Directory.GetFiles(directory, "*", SearchOption.AllDirectories))
        {
            var relativePath = Path.GetRelativePath(directory, file);
            var branchName = relativePath.Replace(Path.DirectorySeparatorChar, '/');
            branches[branchName] = File.GetLastWriteTime(file);
        }
    }

    public static async Task<string> GetBranchDescription(List<GitBranch> branches, string branchName)
    {
        if (!File.Exists(Path.Combine(gitPath, "EDIT_DESCRIPTION"))) return String.Empty;

        var descriptionFile = await File.ReadAllTextAsync(Path.Combine(gitPath, "EDIT_DESCRIPTION"));

        if (!descriptionFile.Contains(branchName)) return String.Empty;

        var lines = descriptionFile.Split('\n');

        var linesWithoutComments = lines.Where(x => !x.StartsWith("#"));

        var description = string.Join(" ", linesWithoutComments);

        return description;
    }

    protected static async Task<bool> ExecuteGitCommand(string gitPath, string arguments)
    {
        var result = await Cli.Wrap("git")
            .WithWorkingDirectory(gitPath)
            .WithArguments(arguments)
            .WithValidation(CommandResultValidation.None)
            .ExecuteAsync();

        return result.ExitCode != 0;
    }

    private static async Task TrySetGitPath()
    {
        try
        {
            const string argument = "rev-parse --git-dir";
            var stdOutBuffer = new StringBuilder();

            var result = await Cli.Wrap("git")
                .WithArguments(argument)
                .WithStandardOutputPipe(PipeTarget.ToStringBuilder(stdOutBuffer))
                .ExecuteAsync();

            var stdOut = stdOutBuffer.ToString().Trim();

            if (string.IsNullOrEmpty(stdOut)) return;

            gitPath = stdOut;
        }
        catch
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("This is not a git repository");
            Console.ResetColor();
            Environment.Exit(1);
        }
    }
}
