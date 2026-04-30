using Bbranch.GitService.Base;
using Bbranch.GitService.OptionStrategies.Common.BranchStrategies;
using Bbranch.Shared.TableData;
using FakeItEasy;

namespace Bbranch.Tests.GitService.Common.BranchStrategies;

[Trait("Category", "Unit")]
public sealed class BranchLocalOptionTests
{
    [Fact]
    public async Task Given_BranchLocalOptions_When_ExecuteRun_Then_Return_LocalBranches()
    {
        var localBranches = new HashSet<GitBranch>
        {
            GitBranch.Default().SetBranch(new Branch("main", true)),
            GitBranch.Default().SetBranch(new Branch("feature/branch", true)),
        };

        var gitRepositoryFake = A.Fake<IGitRepository>();

        A.CallTo(() => gitRepositoryFake.GetLocalBranchNames()).Returns(localBranches);

        var sut = new BranchLocalOptions(gitRepositoryFake);

        HashSet<GitBranch> result = await sut.Execute([]);

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task Given_BranchLocalOptions_When_ExecuteRun_Then_Return_EmptyList_IfNoLocalBranches()
    {
        var gitRepositoryFake = A.Fake<IGitRepository>();

        var branchLocalOptions = new BranchLocalOptions(gitRepositoryFake);

        HashSet<GitBranch> result = await branchLocalOptions.Execute([]);

        Assert.Empty(result);
    }
}
