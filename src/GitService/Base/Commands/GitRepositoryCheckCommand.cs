using System.Diagnostics;

namespace Bbranch.GitService.Base.Commands;

internal class GitRepositoryCheckCommand : AbstractCommand<bool>
{
    public override string CommandArgument => "rev-parse --is-inside-work-tree";

    public override bool Execute()
    {
        using Process process = ExecuteCommand(CommandArgument);

        process.Start();

        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();

        process.WaitForExit();

        return output.Trim() == "true";
    }
}