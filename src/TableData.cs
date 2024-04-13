using System.Globalization;
using Git.Base;
using Git.Options;

namespace TableData;

public record GitBranch(string Name, DateTime LastCommit);

public record BranchTableRow(
    int Ahead,
    int Behind,
    string BranchName,
    (string, string) LastCommit,
    bool IsWorkingBranch,
    string Description
);

public class Project
{
    public static async Task<List<BranchTableRow>> MapGitBranches(
        List<GitBranch> branches,
        string workingBranch,
        string targetBranch = ""
    )
    {
        List<BranchTableRow> branchTable = [];

        foreach (GitBranch branch in branches)
        {
            AheadBehind AheadBehind;

            AheadBehind =
                (!string.IsNullOrEmpty(targetBranch))
                    ? await AheadBehindOptions.GetRemoteAheadBehind(branch.Name, targetBranch)
                    : await AheadBehindOptions.GetRemoteAheadBehind(branch.Name);

            (string commitDate, string timeElapsed) = ParseLastCommit(branch.LastCommit);

            string description = await GitBase.GetBranchDescription(branch.Name);

            if (branch.Name == workingBranch.Trim())
            {
                branchTable.Add(
                    new BranchTableRow(
                        AheadBehind.Ahead,
                        AheadBehind.Behind,
                        branch.Name,
                        (commitDate, timeElapsed),
                        true,
                        description
                    )
                );
                continue;
            }

            branchTable.Add(
                new BranchTableRow(
                    AheadBehind.Ahead,
                    AheadBehind.Behind,
                    branch.Name,
                    (commitDate, timeElapsed),
                    false,
                    description
                )
            );
        }

        return branchTable;
    }

    private static (string commitDate, string timeElapsed) ParseLastCommit(DateTime lastCommit)
    {
        string timeElapsed;
        int days = (DateTime.Now - lastCommit).Days;

        if (days == 0)
        {
            return (lastCommit.ToString("HH:mm", CultureInfo.InvariantCulture), "Today");
        }

        timeElapsed = days == 1 ? "Day ago" : "Days ago";
        return (days.ToString(CultureInfo.InvariantCulture), timeElapsed);
    }
}
