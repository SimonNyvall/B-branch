namespace Bbranch.TableData;

using Bbranch.Info;

public record GitBranch(string Name, DateTime LastCommit);

public record BranchTableRow(int Ahead, int Behind, string BranchName, (string, string) LastCommit, bool IsWorkingBranch, string description);

public class Project
{
    public static List<BranchTableRow> GitBranches(List<GitBranch> branches)
    {
        var branchInfo = new BranchInfo();
        var gitPath = branchInfo.GitPath;

        var branchTable = new List<BranchTableRow>();

        foreach (var branch in branches)
        {
            var (ahead, behind) = branchInfo.GetAheadBehind(gitPath!, branch.Name);

            var (commitDate, timeElapsed) = parseLastCommit(branch.LastCommit);

            var description = branchInfo.GetBranchDescription(gitPath!, branch.Name);

            var workingBranch = branchInfo.TryGetWorkingBranch(gitPath!);

            if (branch.Name == workingBranch)
            {
                branchTable.Add(new BranchTableRow(ahead, behind, branch.Name, (commitDate, timeElapsed), true, description));
                continue;
            }

            branchTable.Add(new BranchTableRow(ahead, behind, branch.Name, (commitDate, timeElapsed), false, description));
        }

        return branchTable;
    }

    private static (string commitDate, string timeElapsed) parseLastCommit(DateTime lastCommit)
    {
        string lastCommitString = String.Empty;
        int days = DateTime.Now.Day - lastCommit.Day;

        if (days == 0)
        {
            lastCommitString = "Today";
            return (lastCommit.ToString("HH:mm"), lastCommitString);
        }

        lastCommitString = days == 1 ? "Day ago" : "Days ago";
        return (days.ToString(), lastCommitString);
    }
}
