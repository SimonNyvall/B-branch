using Bbranch.GitService.Base;
using Bbranch.GitService.OptionStrategies.Common;
using Bbranch.Shared.TableData;
using FakeItEasy;

namespace Tests.GitService;

[Trait("Category", "Unit")]
public sealed class WorkingBranchOptionTests
{
    [Fact]
    public void Given_WorkingBranchOption_When_ExecuteRun_Then_Return_WorkingBranch()
    {
        var branches = new HashSet<GitBranch>
        {
            GitBranch.Default().SetBranch(new Branch("main", false)),
            GitBranch.Default().SetBranch(new Branch("feature", false)),
        };

        var gitRepositoryFake = A.Fake<IGitRepository>();
        A.CallTo(() => gitRepositoryFake.GetWorkingBranch()).Returns("main");

        var workingBranchOption = new WorkingBranchOption(gitRepositoryFake);

        HashSet<GitBranch> result = workingBranchOption.Execute(branches);

        Assert.True(result.First().Branch.IsWorkingBranch);
        Assert.False(result.ElementAt(1).Branch.IsWorkingBranch);
    }

    [Fact]
    public void Given_WorkingBranchOption_When_ExecuteRun_Then_Return_EmptyList()
    {
        HashSet<GitBranch> branches = [];

        var gitRepositoryFake = A.Fake<IGitRepository>();
        A.CallTo(() => gitRepositoryFake.GetWorkingBranch()).Returns("main");

        var workingBranchOption = new WorkingBranchOption(gitRepositoryFake);

        HashSet<GitBranch> result = workingBranchOption.Execute(branches);

        Assert.Empty(result);
    }
}
