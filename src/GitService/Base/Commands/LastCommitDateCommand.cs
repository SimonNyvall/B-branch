using System.Diagnostics;

namespace Git.Base;

internal class LastCommitDateCommand(string branchName) : Command<DateTime>
{
    public override string CommandArgument => $"log -1 --format=%cd --date=iso {branchName}";

    public override DateTime Execute()
    {
        using Process process = ExecuteCommand(CommandArgument);

        process.Start();

        string output = process.StandardOutput.ReadToEnd();

        process.WaitForExit();

        return DateTime.Parse(output);
    }
}