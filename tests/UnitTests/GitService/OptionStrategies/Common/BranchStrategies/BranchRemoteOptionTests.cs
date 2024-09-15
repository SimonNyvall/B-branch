using Bbranch.GitService.Base;
using Bbranch.GitService.OptionStrategies.Common.BranchStrategies;
using Bbranch.Shared.TableData;
using NSubstitute;

namespace Bbranch.Tests.GitService.Common.BranchStrategies;

public class BranchRemoteOptionTests
{
    [Fact]
    public void Execute_ShouldReturnRemoteBranches()
    {
        var mockGitBase = Substitute.For<IGitRepository>();

        var remoteBranches = new List<GitBranch>
        {
            GitBranch.Default().SetBranch(new Branch("origin/main", isWorkingBranch: false)),
            GitBranch.Default().SetBranch(new Branch("origin/feature/branch", isWorkingBranch: false))
        };

        mockGitBase.GetRemoteBranchNames().Returns(remoteBranches);

        var branchRemoteOptions = new BranchRemoteOptions(mockGitBase);

        List<GitBranch> result = branchRemoteOptions.Execute([]);

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void Execute_ShouldReturnRemoteBranches_WithEmptyList()
    {
        var mockGitBase = Substitute.For<IGitRepository>();

        var remoteBranches = new List<GitBranch>();

        mockGitBase.GetRemoteBranchNames().Returns(remoteBranches);

        var branchRemoteOptions = new BranchRemoteOptions(mockGitBase);

        List<GitBranch> result = branchRemoteOptions.Execute([]);

        Assert.Empty(result);
    }
}