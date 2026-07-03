using System.Diagnostics;
using System.Globalization;
using Bbranch.GitService.Base.Commands;
using Bbranch.Shared.TableData;

internal sealed class LastPackedCommitDateFetchCommand(List<GitBranch> branches)
    : AbstractCommand<Task<List<GitBranch>>>
{
    public override string CommandArgument =>
        "for-each-ref refs/remotes/origin --format=\"%(refname)|%(committerdate:iso8601)\"";

    public override async Task<List<GitBranch>> Execute()
    {
        using Process process = ExecuteCommand(CommandArgument);

        process.Start();

        string output = await process.StandardOutput.ReadToEndAsync();

        await process.WaitForExitAsync();

        var branchLookup = branches.ToDictionary(b => $"refs/remotes/{b.Branch.Name}");

        foreach (var line in output.Split('\n', StringSplitOptions.RemoveEmptyEntries))
        {
            var parts = line.Split('|', 2);

            if (parts.Length != 2)
                continue;

            if (!branchLookup.TryGetValue(parts[0], out var branch))
                continue;

            if (
                DateTime.TryParse(
                    parts[1],
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeLocal,
                    out var date
                )
            )
            {
                branch.SetLastCommit(date);
            }
        }

        return branches;
    }
}
