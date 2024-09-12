using System.Diagnostics;

namespace Git.Base;

internal class TrackAheadBehindCommand(string localBranchName, string remoteBranchName) : Command<string>
{
    public override string CommandArgument => $"rev-list --left-right --count {localBranchName}...{remoteBranchName}";

    public override string Execute()
    {
        using Process process = ExecuteCommand(CommandArgument);

        process.Start();

        string output = process.StandardOutput.ReadToEnd();

        process.WaitForExit();

        if (string.IsNullOrWhiteSpace(output)) return string.Empty;

        return output;
    }
}