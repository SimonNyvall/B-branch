using Bbranch.GitService.OptionStrategies.Common.SortStrategies;
using Bbranch.Shared.TableData;

namespace Bbranch.Tests.GitService.Common.SortStrategies;

[Trait("Category", "Unit")]
public class SortByLastCommitOptionTests()
{
    [Fact]
    public async Task Given_SortByLastCommitOptions_When_ExecuteRun_Then_Return_SortedBranches()
    {
        var currentDateTime = DateTime.Now;

        var branches = new HashSet<GitBranch>
        {
            GitBranch.Default().SetLastCommit(currentDateTime),
            GitBranch.Default().SetLastCommit(currentDateTime.AddDays(-1)),
            GitBranch.Default().SetLastCommit(currentDateTime.AddDays(-2)),
        };

        var sortByLastCommitOption = new SortByLastCommitOptions();

        var result = await sortByLastCommitOption.Execute(branches);

        Assert.Equal(currentDateTime.AddDays(-2), result.ElementAt(2).LastCommit);
        Assert.Equal(currentDateTime.AddDays(-1), result.ElementAt(1).LastCommit);
        Assert.Equal(currentDateTime, result.First().LastCommit);
    }

    [Fact]
    public async Task Given_SortByLastCommitOptions_When_ExecuteRun_Then_Return_SortedBranches_WhenAlreadySorted()
    {
        var currentDateTime = DateTime.Now;

        var branches = new HashSet<GitBranch>
        {
            GitBranch.Default().SetLastCommit(currentDateTime.AddDays(-2)),
            GitBranch.Default().SetLastCommit(currentDateTime.AddDays(-1)),
            GitBranch.Default().SetLastCommit(currentDateTime),
        };

        var sortByLastCommitOption = new SortByLastCommitOptions();

        var result = await sortByLastCommitOption.Execute(branches);

        Assert.Equal(currentDateTime.AddDays(-2), result.ElementAt(2).LastCommit);
        Assert.Equal(currentDateTime.AddDays(-1), result.ElementAt(1).LastCommit);
        Assert.Equal(currentDateTime, result.First().LastCommit);
    }
}
