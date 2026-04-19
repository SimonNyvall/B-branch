using Bbranch.GitService.Base;
using Bbranch.GitService.OptionStrategies.Common.BranchStrategies;
using Bbranch.Shared.TableData;
using FakeItEasy;

namespace Bbranch.Tests.GitService.Common.BranchStrategies;

[Trait("Category", "Unit")]
public sealed class BranchRemoteOptionTests
{
    [Fact]
    public void Given_BranchRemoteOptions_When_ExecuteRun_Then_Return_RemoteBranches()
    {
        var remoteBranches = new HashSet<GitBranch>
        {
            GitBranch.Default().SetBranch(new Branch("origin/main", false)),
            GitBranch.Default().SetBranch(new Branch("origin/feature/branch", false)),
        };

        var gitRepositoryFake = A.Fake<IGitRepository>();
        A.CallTo(() => gitRepositoryFake.GetRemoteBranchNames()).Returns(remoteBranches);

        var sut = new BranchRemoteOptions(gitRepositoryFake);

        HashSet<GitBranch> result = sut.Execute([]);

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void Given_BranchRemoteOptions_When_ExecuteRun_Then_Return_EmptyList_IfNoRemoteBranches()
    {
        var gitRepositoryFake = A.Fake<IGitRepository>();
        var branchRemoteOptions = new BranchRemoteOptions(gitRepositoryFake);

        HashSet<GitBranch> result = branchRemoteOptions.Execute([]);

        Assert.Empty(result);
    }
}
