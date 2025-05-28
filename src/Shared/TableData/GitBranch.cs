namespace Bbranch.Shared.TableData;

public sealed class GitBranch
{
    public Branch Branch { get; private set; } = null!;
    public AheadBehind AheadBehind { get; private set; }
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

    public GitBranch SetAheadBehind(AheadBehind aheadBehind)
    {
        if (aheadBehind.Ahead < 0 || aheadBehind.Behind < 0)
        {
            throw new ArgumentException("Ahead and Behind should be positive integers");
        }

        AheadBehind = aheadBehind;

        return this;
    }

    public GitBranch SetBranch(Branch branch)
    {
        if (string.IsNullOrEmpty(branch.Name))
        {
            throw new ArgumentException("Branch name should not be empty");
        }

        Branch = branch;

        return this;
    }

    public GitBranch SetLastCommit(DateTime lastCommit)
    {
        if (lastCommit > DateTime.Now)
        {
            throw new ArgumentException("Last commit date cannot be in the future");
        }

        LastCommit = lastCommit;

        return this;
    }

    public GitBranch SetDescription(string description)
    {
        Description = description ?? string.Empty;

        return this;
    }

    public static GitBranch Default()
    {
        return new GitBranch(
            new AheadBehind(0, 0),
            new Branch("branchName", false),
            DateTime.MinValue,
            string.Empty
        );
    }
}
