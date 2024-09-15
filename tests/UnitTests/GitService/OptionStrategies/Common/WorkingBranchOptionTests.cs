using Bbranch.GitService.Base;
using Bbranch.GitService.OptionStrategies.Common;
using Bbranch.Shared.TableData;
using NSubstitute;

namespace Tests.GitService;

public class WorkingBranchOptionTests
{
    [Fact]
    public void Execute_ShouldReturnWorkingBranch_WhenBranchesAreProvided()
    {
        var mockGitBase = Substitute.For<IGitRepository>();
        mockGitBase.GetWorkingBranch().Returns("main");

        var branches = new List<GitBranch>
        {
            GitBranch.Default().SetBranch(new Branch("main", isWorkingBranch: false)),
            GitBranch.Default().SetBranch(new Branch("feature", isWorkingBranch: false))
        };

        var workingBranchOption = new WorkingBranchOption(mockGitBase);

        List<GitBranch> result = workingBranchOption.Execute(branches);

        Assert.True(result[0].Branch.IsWorkingBranch);
        Assert.False(result[1].Branch.IsWorkingBranch);
    }

    [Fact]
    public void Execute_ShouldReturnEmptyList_WhenEmptyBranchListAreProvided()
    {
        var mockGitBase = Substitute.For<IGitRepository>();
        mockGitBase.GetWorkingBranch().Returns("main");

        List<GitBranch> branches = [];

        var workingBranchOption = new WorkingBranchOption(mockGitBase);

        List<GitBranch> result = workingBranchOption.Execute(branches);

        Assert.Empty(result);
    }
}