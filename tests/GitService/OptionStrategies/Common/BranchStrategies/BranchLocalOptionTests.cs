using Git.Base;
using Git.Options;
using Shared.TableData;
using NSubstitute;

namespace Tests.GitService;

public class BranchLocalOptionTests
{
    [Fact]
    public void Execute_ShouldReturnLocalBranches()
    {
        var mockGitBase = Substitute.For<IGitBase>();

        var localBranches = new List<GitBranch>
        {
            GitBranch.Default().SetBranch(new Branch("main", isWorkingBranch: true)),
            GitBranch.Default().SetBranch(new Branch("feature/branch", isWorkingBranch: true))
        };

        mockGitBase.GetLocalBranchNames().Returns(localBranches);

        var branchLocalOptions = new BranchLocalOptions(mockGitBase);

        List<GitBranch> result = branchLocalOptions.Execute([]);

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void Execute_ShouldReturnLocalBranches_WithEmptyList()
    {
        var mockGitBase = Substitute.For<IGitBase>();

        var localBranches = new List<GitBranch>();

        mockGitBase.GetLocalBranchNames().Returns(localBranches);

        var branchLocalOptions = new BranchLocalOptions(mockGitBase);

        List<GitBranch> result = branchLocalOptions.Execute([]);

        Assert.Empty(result);
    }
}