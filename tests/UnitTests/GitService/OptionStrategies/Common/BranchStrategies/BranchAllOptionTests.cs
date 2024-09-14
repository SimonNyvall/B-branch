using Bbranch.GitService.Base;
using Bbranch.GitService.OptionStrategies.Common.BranchStrategies;
using Bbranch.Shared.TableData;
using NSubstitute;

namespace Bbranch.Tests.GitService.Common.BranchStrategies;

public class BranchAllOptionTests
{
    [Fact]
    public void Execute_ShouldReturnAllBranches()
    {
        var mockGitBase = Substitute.For<IGitRepository>();

        var localBranches = new List<GitBranch>
        {
            GitBranch.Default().SetBranch(new Branch("main", isWorkingBranch: true)),
            GitBranch.Default().SetBranch(new Branch("feature/branch", isWorkingBranch: false))
        };

        var remoteBranches = new List<GitBranch>
        {
            GitBranch.Default().SetBranch(new Branch("origin/main", isWorkingBranch: false)),
            GitBranch.Default().SetBranch(new Branch("origin/feature/branch", isWorkingBranch: false))
        };

        mockGitBase.GetLocalBranchNames().Returns(localBranches);

        mockGitBase.GetRemoteBranchNames().Returns(remoteBranches);

        var branchAllOptions = new BranchAllOptions(mockGitBase);

        List<GitBranch> result = branchAllOptions.Execute(new List<GitBranch>());

        Assert.Equal(4, result.Count);
    }

    [Fact]
    public void Execute_ShouldReturnAllBranches_WithEmptyList()
    {
        var mockGitBase = Substitute.For<IGitRepository>();

        var localBranches = new List<GitBranch>();

        var remoteBranches = new List<GitBranch>();

        mockGitBase.GetLocalBranchNames().Returns(localBranches);

        mockGitBase.GetRemoteBranchNames().Returns(remoteBranches);

        var branchAllOptions = new BranchAllOptions(mockGitBase);

        List<GitBranch> result = branchAllOptions.Execute([]);

        Assert.Empty(result);
    }
}