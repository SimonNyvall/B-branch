namespace Bbranch.Shared.TableData;

public sealed class GitBranch
{
    private const int ShortCommitHashLength = 7;
    public Branch Branch { get; private set; } = null!;
    public AheadBehind AheadBehind { get; private set; }
    public DateTime LastCommit { get; private set; }
    public string? Description { get; private set; }
    public DetachedHead DetachedHead { get; private set; }
    public bool IsRemote { get; private set; }
    public bool IsSymbolic
    {
        get => SymLink != null;
    }
    public Symbolic? SymLink { get; private set; } = null;

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
        DetachedHead = new DetachedHead(null);
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
        Description = description;

        return this;
    }

    public GitBranch SetDetachedHead(string commitHash)
    {
        if (string.IsNullOrEmpty(commitHash))
        {
            throw new ArgumentException("Commit hash cannot be null nor empty");
        }

        if (commitHash.Length != ShortCommitHashLength)
        {
            throw new ArgumentException($"Commit hash must be {ShortCommitHashLength} chats long");
        }

        var detachedHead = new DetachedHead(commitHash);
        DetachedHead = detachedHead;

        return this;
    }

    public GitBranch SetIsRemote(bool isRemote)
    {
        IsRemote = isRemote;
        return this;
    }

    public GitBranch SetSymLink(Symbolic symLink)
    {
        SymLink = symLink;
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
