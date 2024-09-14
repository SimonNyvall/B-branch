using Bbranch.Shared.TableData;

namespace Bbranch.Tests.Shared;

public class TableDataTests
{
    [Fact]
    public void GitBranch_ShouldSetAheadBehind_WithValidAheadBehind()
    {
        AheadBehind aheadBehind = new(1, 2);

        GitBranch gitBranch = GitBranch.Default().SetAheadBehind(aheadBehind);
        
        Assert.Equal(aheadBehind, gitBranch.AheadBehind);
    }

    [Fact]
    public void GitBranch_ShouldThrowException_WithNegativeAheadBehind()
    {
        AheadBehind aheadBehind = new(-1, 2);

        Assert.Throws<ArgumentException>(() => GitBranch.Default().SetAheadBehind(aheadBehind));
    }

    [Fact]
    public void GitBranch_ShouldSetBranch_WithValidBranch()
    {
        Branch branch = new("main", isWorkingBranch: true);

        GitBranch gitBranch = GitBranch.Default().SetBranch(branch);

        Assert.Equal(branch, gitBranch.Branch);
    }

    [Fact]
    public void GitBranch_ShouldThrowException_WithNameIsNull()
    {
        Branch branch = new(string.Empty, isWorkingBranch: true);

        Assert.Throws<ArgumentException>(() => GitBranch.Default().SetBranch(branch));
    }

    [Fact]
    public void GitBranch_ShouldSetDescription_WithValidDescription()
    {
        string description = "This is a description";

        GitBranch gitBranch = GitBranch.Default().SetDescription(description);

        Assert.Equal(description, gitBranch.Description);
    }

    [Fact]
    public void GitBranch_ShouldSetLastCommit_WithValidLastCommit()
    {
        DateTime lastCommit = DateTime.Now;

        GitBranch gitBranch = GitBranch.Default().SetLastCommit(lastCommit);

        Assert.Equal(lastCommit, gitBranch.LastCommit);
    }

    [Fact]
    public void GitBranch_ShouldThrowException_WithDateTimeMinValue()
    {
        DateTime lastCommit = DateTime.MinValue;

        Assert.Throws<ArgumentException>(() => GitBranch.Default().SetLastCommit(lastCommit));
    }
}
