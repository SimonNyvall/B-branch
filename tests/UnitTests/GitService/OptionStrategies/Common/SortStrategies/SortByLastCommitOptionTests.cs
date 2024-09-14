using Bbranch.GitService.OptionStrategies.Common.SortStrategies;
using Bbranch.Shared.TableData;

namespace Bbranch.Tests.GitService.Common.SortStrategies;

public class SortByLastCommitOptionTests()
{
    [Fact]
    public void SortByLastCommitOption_ShouldReturnSortedBranches()
    {
        var currentDateTime = DateTime.Now;

        var branches = new List<GitBranch>
        {
            GitBranch.Default().SetLastCommit(currentDateTime),
            GitBranch.Default().SetLastCommit(currentDateTime.AddDays(-1)),
            GitBranch.Default().SetLastCommit(currentDateTime.AddDays(-2)),
        };

        var sortByLastCommitOption = new SortByLastCommitOptions();

        var result = sortByLastCommitOption.Execute(branches);

        Assert.Equal(currentDateTime.AddDays(-2), result[2].LastCommit);
        Assert.Equal(currentDateTime.AddDays(-1), result[1].LastCommit);
        Assert.Equal(currentDateTime, result[0].LastCommit);
    }

    [Fact]
    public void SortByLastCommitOption_ShouldReturnSortedBranches_WhenBranchesAreAlreadySorted()
    {
        var currentDateTime = DateTime.Now;

        var branches = new List<GitBranch>
        {
            GitBranch.Default().SetLastCommit(currentDateTime.AddDays(-2)),
            GitBranch.Default().SetLastCommit(currentDateTime.AddDays(-1)),
            GitBranch.Default().SetLastCommit(currentDateTime),
        };

        var sortByLastCommitOption = new SortByLastCommitOptions();

        var result = sortByLastCommitOption.Execute(branches);

        Assert.Equal(currentDateTime.AddDays(-2), result[2].LastCommit);
        Assert.Equal(currentDateTime.AddDays(-1), result[1].LastCommit);
        Assert.Equal(currentDateTime, result[0].LastCommit);
    }
}