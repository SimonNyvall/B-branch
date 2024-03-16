namespace Bbranch.Git.Options.AheadBehind;

using Bbranch.Git.Base.GitBase;
using System.Text;
using System.Text.RegularExpressions;
using CliWrap;

public struct AheadBehind
{
    public int Ahead { get; }
    public int Behind { get; }

    public AheadBehind(int ahead, int behind)
    {
        Ahead = ahead;
        Behind = behind;
    }
}

public class AheadBehindOptions : GitBase
{
    public static async Task<AheadBehind> GetRemoteAheadBehind(string brancName)
    {
        return await GetAheadBehind(brancName, brancName);
    }

    public static async Task<AheadBehind> GetRemoteAheadBehind(string branchName, string checkAgainst)
    {
        return await GetAheadBehind(branchName, checkAgainst);
    }

    private static async Task<AheadBehind> GetAheadBehind(string localBranchName, string remoteBranchName)
    {
        int ahead = 0, behind = 0;

        string checkLocalBranchCommand = $"rev-parse --verify {localBranchName}";
        string checkRemoteBranchCommand = $"rev-parse --verify origin/{remoteBranchName}";

        if (await ExecuteGitCommand(gitPath, checkLocalBranchCommand) || await ExecuteGitCommand(gitPath, checkRemoteBranchCommand))
        {
            return new AheadBehind(ahead, behind);
        }

        string arguments = $"rev-list --left-right --count {localBranchName}...origin/{remoteBranchName}";

        var stdOutBuffer = new StringBuilder();

        var result = await Cli.Wrap("git")
            .WithArguments(arguments)
            .WithStandardOutputPipe(PipeTarget.ToStringBuilder(stdOutBuffer))
            .WithWorkingDirectory(gitPath)
            .ExecuteAsync();

        var stdOut = stdOutBuffer.ToString();

        if (String.IsNullOrEmpty(stdOut)) return new AheadBehind(ahead, behind);

        if (result.ExitCode != 0) return new AheadBehind(ahead, behind);

        const string pattern = @"(\d+)\s+(\d+)";

        Match match = Regex.Match(stdOut, pattern);

        if (!match.Success) return new AheadBehind(ahead, behind);

        ahead = int.Parse(match.Groups[1].Value);
        behind = int.Parse(match.Groups[2].Value);

        return new AheadBehind(ahead, behind);
    }
}


