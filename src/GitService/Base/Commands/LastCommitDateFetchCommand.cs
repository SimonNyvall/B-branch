using System.Diagnostics;

namespace Bbranch.GitService.Base.Commands;

internal sealed class LastCommitDateFetchCommand(string branchName) : AbstractCommand<DateTime>
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