using System.Globalization;
using Git.Base;
using Git.Options;

namespace TableData;

public struct Branch
{
    public string Name { get; set; }
    public bool IsWorkingBranch { get; set; }
}

public struct AheadBehind
{
    public int Ahead { get; set; }
    public int Behind { get; set; }
}

public class GitBranch
{
    public AheadBehind AheadBehind { get; private set; }
    public Branch Branch { get; private set; }
    public DateTime LastCommit { get; private set; }
    public string? Description { get; private set; }

    public GitBranch(
        AheadBehind aheadBehind,
        Branch branch,
        DateTime lastCommit,
        string description
    )
    {
        SetAheadBehind(aheadBehind);
        SetBranch(branch);
        SetLastCommit(lastCommit);
        SetDescription(description);
    }

    public void SetAheadBehind(AheadBehind aheadBehind)
    {
        if (aheadBehind.Ahead < 0 || aheadBehind.Behind < 0)
        {
            throw new ArgumentException("Ahead and Behind should be positive integers");
        }

        AheadBehind = aheadBehind;
    }

    public void SetBranch(Branch branch)
    {
        if (string.IsNullOrEmpty(branch.Name))
        {
            throw new ArgumentException("Branch name should not be empty");
        }

        Branch = branch;
    }

    public void SetLastCommit(DateTime lastCommit)
    {
        if (lastCommit == DateTime.MinValue)
        {
            throw new ArgumentException("Last commit should not be empty");
        }

        LastCommit = lastCommit;
    }

    public void SetDescription(string description)
    {
        if (description == null)
        {
            throw new ArgumentException("Description should not be empty");
        }

        Description = description;
    }
}

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
