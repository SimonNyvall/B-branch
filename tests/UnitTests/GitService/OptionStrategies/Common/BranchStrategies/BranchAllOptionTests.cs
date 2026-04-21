using Bbranch.GitService.Base;
using Bbranch.GitService.OptionStrategies.Common.BranchStrategies;
using Bbranch.Shared.TableData;
using FakeItEasy;

namespace Bbranch.Tests.GitService.Common.BranchStrategies;

[Trait("Category", "Unit")]
public class BranchAllOptionTests
{
    [Fact]
    public void Given_BranchAllOptions_When_ExecuteRun_Then_Return_AllBranches()
    {
        var localBranches = new HashSet<GitBranch>
        {
            GitBranch.Default().SetBranch(new Branch("main", true)),
            GitBranch.Default().SetBranch(new Branch("feature/branch", false)),
        };

        var remoteBranches = new HashSet<GitBranch>
        {
            GitBranch.Default().SetBranch(new Branch("origin/main", false)),
            GitBranch.Default().SetBranch(new Branch("origin/feature/branch", false)),
        };

        var gitRepositoryFake = A.Fake<IGitRepository>();

        A.CallTo(() => gitRepositoryFake.GetLocalBranchNames()).Returns(localBranches);
        A.CallTo(() => gitRepositoryFake.GetRemoteBranchNames()).Returns(remoteBranches);

        var branchAllOptions = new BranchAllOptions(gitRepositoryFake);

        HashSet<GitBranch> result = branchAllOptions.Execute([]);

        Assert.Equal(4, result.Count);
    }

    [Fact]
    public void Given_BranchAllOptions_When_ExecuteRun_Then_Return_EmptyList_IfNoBranches()
    {
        var gitRepositoryFake = A.Fake<IGitRepository>();
        var sut = new BranchAllOptions(gitRepositoryFake);

        HashSet<GitBranch> result = sut.Execute([]);

        Assert.Empty(result);
    }
}
