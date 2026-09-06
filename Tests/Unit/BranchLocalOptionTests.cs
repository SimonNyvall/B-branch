using Bbranch.GitService.Base;
using Bbranch.GitService.OptionStrategies.Common.BranchStrategies;
using Bbranch.Shared.TableData;
using FakeItEasy;

namespace Bbranch.Tests.Unit;

[Trait("Category", "Unit")]
public sealed class BranchLocalOptionTests
{
    [Fact]
    public async Task Given_BranchLocalOptions_When_ExecuteRun_Then_Return_LocalBranches()
    {
        var localBranches = new List<GitBranch>
        {
            GitBranch.Default().SetBranch(new BranchViewModel("main", true)),
            GitBranch.Default().SetBranch(new BranchViewModel("feature/branch", true)),
        };

        var gitRepositoryFake = A.Fake<IInternalGitRepository>();

        A.CallTo(() => gitRepositoryFake.GetLocalBranchNames()).Returns(localBranches);

        var sut = new BranchLocalOptions(gitRepositoryFake);

        List<GitBranch> result = await sut.Execute([]);

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task Given_BranchLocalOptions_When_ExecuteRun_Then_Return_EmptyList_IfNoLocalBranches()
    {
        var gitRepositoryFake = A.Fake<IInternalGitRepository>();

        var branchLocalOptions = new BranchLocalOptions(gitRepositoryFake);

        List<GitBranch> result = await branchLocalOptions.Execute([]);

        Assert.Empty(result);
    }
}
