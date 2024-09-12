using System.Diagnostics;

namespace Git.Base;

internal class BranchExistenceCheckCommand(string branchName) : AbstractCommand<bool>
{
    public override string CommandArgument => $"rev-parse --verify {branchName}";

    public override bool Execute()
    {
        using Process process = ExecuteCommand(CommandArgument);

        process.Start();

        string output = process.StandardOutput.ReadToEnd();

        process.WaitForExit();

        return !string.IsNullOrWhiteSpace(output);
    }
}