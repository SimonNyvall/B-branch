namespace Bbranch.Info;

using System.Text;
using System.Text.RegularExpressions;
using Bbranch.TableData;
using CliWrap;

public class BranchInfo
{
    public static string? GitPath { get; private set; } = null;

    public static async Task Initialize()
    {
        await TrySetGitPath();
    }

    public static async Task<(int Ahead, int Behind)> GetAheadBehind(string gitPath, string branchName)
    {
        int ahead = 0, behind = 0;

        string checkLocalBranchCommand = $"rev-parse --verify {branchName}";
        string checkRemoteBranchCommand = $"rev-parse --verify origin/{branchName}";

        if (await ExecuteGitCommand(gitPath, checkLocalBranchCommand) || await ExecuteGitCommand(gitPath, checkRemoteBranchCommand))
        {
            return (ahead, behind);
        }

        string arguments = $"rev-list --left-right --count {branchName}...origin/{branchName}";

        var stdOutBuffer = new StringBuilder();

        var result = await Cli.Wrap("git")
            .WithArguments(arguments)
            .WithStandardOutputPipe(PipeTarget.ToStringBuilder(stdOutBuffer))
            .WithWorkingDirectory(gitPath)
            .ExecuteAsync();

        var stdOut = stdOutBuffer.ToString();

        if (String.IsNullOrEmpty(stdOut)) return (ahead, behind);

        if (result.ExitCode != 0) return (ahead, behind);

        const string pattern = @"(\d+)\s+(\d+)";

        Match match = Regex.Match(stdOut, pattern);

        if (!match.Success) return (ahead, behind);

        ahead = int.Parse(match.Groups[1].Value);
        behind = int.Parse(match.Groups[2].Value);

        return (ahead, behind);
    }

    public static string? TryGetWorkingBranch(string gitPath)
    {
        try
        {
            return GetWorkingBranch(gitPath);
        }
        catch
        {
            return null;
        }
    }

    private static string? GetWorkingBranch(string gitPath)
    {
        var HEADFile = File.ReadAllText(Path.Combine(gitPath, "HEAD")).Trim();

        if (HEADFile.StartsWith("ref:"))
        {
            var branchNameComponents = HEADFile.Split('/');

            var branchName = string.Join("/", branchNameComponents.Skip(2));

            return branchName;
        }

        return null;
    }

    public static List<GitBranch> GetNamesAndLastWirte(string gitPath)
    {
        var branches = new Dictionary<string, DateTime>();
        var branchDir = Path.Combine(gitPath, "refs", "heads");

        if (!Directory.Exists(branchDir)) throw new Exception("Branch directory does not exist");

        foreach (var file in Directory.GetFiles(branchDir, "*", SearchOption.AllDirectories))
        {
            var relativePath = Path.GetRelativePath(branchDir, file);
            var branchName = relativePath.Replace(Path.DirectorySeparatorChar, '/');
            branches.Add(branchName, File.GetLastWriteTime(file));
        }

        return branches
            .Select(x => new GitBranch(x.Key, x.Value))
            .OrderByDescending(x => x.LastCommit)
            .ToList();
    }

    public static async Task<string> GetBranchDescription(string gitPath, string branchName)
    {
        if (!File.Exists(Path.Combine(gitPath, "EDIT_DESCRIPTION"))) return String.Empty;

        var descriptionFile = await File.ReadAllTextAsync(Path.Combine(gitPath, "EDIT_DESCRIPTION"));

        var branches = GetNamesAndLastWirte(gitPath);

        if (!descriptionFile.Contains(branchName)) return String.Empty;

        var lines = descriptionFile.Split('\n');

        var linesWithoutComments = lines.Where(x => !x.StartsWith("#"));

        var description = string.Join(" ", linesWithoutComments);

        return description;
    }

    private static async Task<bool> ExecuteGitCommand(string gitPath, string arguments)
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
        const string argument = "rev-parse --git-dir";
        var stdOutBuffer = new StringBuilder();

        var result = await Cli.Wrap("git")
            .WithArguments(argument)
            .WithStandardOutputPipe(PipeTarget.ToStringBuilder(stdOutBuffer))
            .ExecuteAsync();

        var stdOut = stdOutBuffer.ToString().Trim();

        if (string.IsNullOrEmpty(stdOut)) return;

        GitPath = stdOut;
    }
}
