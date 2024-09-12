using System.Diagnostics;

namespace Git.Base;

internal class GitDirectoryLocationCommand : Command<string>
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