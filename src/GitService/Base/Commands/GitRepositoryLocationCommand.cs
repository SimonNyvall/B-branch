using System.Diagnostics;

namespace Bbranch.GitService.Base.Commands;

internal class GitRepositoryLocationCommand : AbstractCommand<string>
{
    public override string CommandArgument => "rev-parse --git-dir";

    public override string Execute()
    {
        using Process process = ExecuteCommand(CommandArgument);

        process.Start();

        string output = process.StandardOutput.ReadToEnd();

        process.WaitForExit();

        if (string.IsNullOrWhiteSpace(output)) return string.Empty;

        return output.Trim();
    }
}