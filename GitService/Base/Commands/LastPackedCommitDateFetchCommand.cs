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

        var branchLookup = branches.ToDictionary(b => $"refs/remotes/{b.Branch.Name}");

        while (await process.StandardOutput.ReadLineAsync() is { } line)
        {
            int separator = line.IndexOf('|');
            if (separator < 0)
                continue;

            ReadOnlySpan<char> branchRef = line.AsSpan(0, separator);
            ReadOnlySpan<char> dateText = line.AsSpan(separator + 1);

            if (!branchLookup.TryGetValue(branchRef.ToString(), out var branch))
                continue;

            if (
                DateTime.TryParse(
                    dateText,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeLocal,
                    out var date
                )
            )
            {
                branch.SetLastCommit(date);
            }
        }

        await process.WaitForExitAsync();
        return branches;
    }
}
