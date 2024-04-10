using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using CliWrap;
using Git.Base;

namespace Git.Options
{
    public readonly struct AheadBehind(int ahead, int behind)
    {
        public int Ahead => ahead;
        public int Behind => behind;
    }

    public partial class AheadBehindOptions : GitBase
    {
        public static async Task<AheadBehind> GetRemoteAheadBehind(string brancName)
        {
            return await GetAheadBehind(brancName, brancName);
        }

        public static async Task<AheadBehind> GetRemoteAheadBehind(
            string branchName,
            string checkAgainst
        )
        {
            return await GetAheadBehind(branchName, checkAgainst);
        }

        private static async Task<AheadBehind> GetAheadBehind(
            string localBranchName,
            string remoteBranchName
        )
        {
            int ahead = 0,
                behind = 0;

            string checkLocalBranchCommand = $"rev-parse --verify {localBranchName}";
            string checkRemoteBranchCommand = $"rev-parse --verify origin/{remoteBranchName}";

            if (
                await ExecuteGitCommand(GitPath, checkLocalBranchCommand)
                || await ExecuteGitCommand(GitPath, checkRemoteBranchCommand)
            )
            {
                return new AheadBehind(ahead, behind);
            }

            string arguments =
                $"rev-list --left-right --count {localBranchName}...origin/{remoteBranchName}";

            StringBuilder stdOutBuffer = new();

            CommandResult result = await Cli.Wrap("git")
                .WithArguments(arguments)
                .WithStandardOutputPipe(PipeTarget.ToStringBuilder(stdOutBuffer))
                .WithValidation(CommandResultValidation.None)
                .WithWorkingDirectory(GitPath)
                .ExecuteAsync();

            string stdOut = stdOutBuffer.ToString();

            if (string.IsNullOrEmpty(stdOut))
            {
                return new AheadBehind(ahead, behind);
            }

            if (result.ExitCode != 0)
            {
                LogError("Error getting ahead/behind information");
                return new AheadBehind(ahead, behind);
            }

            Match match = GeneratedRegex().Match(stdOut);
            if (match.Success)
            {
                ahead = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                behind = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
                return new AheadBehind(ahead, behind);
            }

            LogError("Error getting ahead/behind information");
            return new AheadBehind(ahead, behind);
        }

        [GeneratedRegex(@"(\d+)\s+(\d+)", RegexOptions.Compiled)]
        private static partial Regex GeneratedRegex();
    }
}
