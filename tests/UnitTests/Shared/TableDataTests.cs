using Bbranch.Shared.TableData;

namespace Bbranch.Tests.Shared;

public sealed class TableDataTests
{
    [Fact]
    public void Given_ValidAheadBehind_When_SetAheadBehindRun_Then_Set_AheadBehind()
    {
        AheadBehind aheadBehind = new(1, 2);

        var gitBranch = GitBranch.Default().SetAheadBehind(aheadBehind);
        
        Assert.Equal(aheadBehind, gitBranch.AheadBehind);
    }

    [Fact]
    public void Given_InvalidAheadBehind_When_SetAheadBehindRun_Then_Throw_ArgumentException()
    {
        AheadBehind aheadBehind = new(-1, 2);

        Assert.Throws<ArgumentException>(() => GitBranch.Default().SetAheadBehind(aheadBehind));
    }

    [Fact]
    public void Given_ValidBranch_When_SetBranchRun_Then_Set_Branch()
    {
        Branch branch = new("main", isWorkingBranch: true);

        GitBranch gitBranch = GitBranch.Default().SetBranch(branch);

        Assert.Equal(branch, gitBranch.Branch);
    }

    [Fact]
    public void Given_InvalidBranch_When_SetBranchRun_Then_Throw_ArgumentException()
    {
        Branch branch = new(string.Empty, isWorkingBranch: true);

        Assert.Throws<ArgumentException>(() => GitBranch.Default().SetBranch(branch));
    }

    [Fact]
    public void Given_Description_When_SetDescriptionRun_Then_Set_Description()
    {
        var description = "This is a description";

        GitBranch gitBranch = GitBranch.Default().SetDescription(description);

        Assert.Equal(description, gitBranch.Description);
    }

    [Fact]
    public void Given_ValidLastCommitDate_When_SetLastCommitRun_Then_Set_LastCommit()
    {
        DateTime lastCommit = DateTime.Now;

        GitBranch gitBranch = GitBranch.Default().SetLastCommit(lastCommit);

        Assert.Equal(lastCommit, gitBranch.LastCommit);
    }

    [Fact]
    public void Given_InvalidLastCommitDate_When_SetLastCommitRun_Then_Throw_ArgumentException()
    {
        DateTime lastCommit = DateTime.MaxValue;

        Assert.Throws<ArgumentException>(() => GitBranch.Default().SetLastCommit(lastCommit));
    }
}
