namespace Bbranch.TableData;

using Bbranch.Git.Base.GitBase;
using Bbranch.Git.Options.AheadBehind;

public record GitBranch(string Name, DateTime LastCommit);

public record BranchTableRow(int Ahead, int Behind, string BranchName, (string, string) LastCommit, bool IsWorkingBranch, string description);

public class Project
{
    public static async Task<List<BranchTableRow>> MapGitBranches(GitBase gitBase, List<GitBranch> branches, string workingBranch, string targetBranch = "")
    {
        var branchTable = new List<BranchTableRow>();

        foreach (var branch in branches)
        {
            var AheadBehind = new AheadBehind();

            if (!String.IsNullOrEmpty(targetBranch))
            {
                AheadBehind = await AheadBehindOptions.GetRemoteAheadBehind(branch.Name, targetBranch);
            }
            else
            {
                AheadBehind = await AheadBehindOptions.GetRemoteAheadBehind(branch.Name);
            }

            var (commitDate, timeElapsed) = parseLastCommit(branch.LastCommit);

            var description = await GitBase.GetBranchDescription(branches, branch.Name);

            if (branch.Name == workingBranch.Trim())
            {
                branchTable.Add(new BranchTableRow(AheadBehind.Ahead, AheadBehind.Behind, branch.Name, (commitDate, timeElapsed), true, description));
                continue;
            }

            branchTable.Add(new BranchTableRow(AheadBehind.Ahead, AheadBehind.Behind, branch.Name, (commitDate, timeElapsed), false, description));
        }

        return branchTable;
    }

    private static (string commitDate, string timeElapsed) parseLastCommit(DateTime lastCommit)
    {
        string timeElapsed = String.Empty;
        int days = (DateTime.Now - lastCommit).Days;

        if (days == 0) return (lastCommit.ToString("HH:mm"), "Today");

        timeElapsed = days == 1 ? "Day ago" : "Days ago";
        return (days.ToString(), timeElapsed);
    }
}
