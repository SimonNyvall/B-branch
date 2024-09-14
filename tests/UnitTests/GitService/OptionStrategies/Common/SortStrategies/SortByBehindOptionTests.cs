using Bbranch.GitService.OptionStrategies.Common.SortStrategies;
using Bbranch.Shared.TableData;

namespace Bbranch.Tests.GitService.Common.SortStrategies;

public class SortByAheadOptionTests
{
    [Fact]
    public void SortByAheadOption_ShouldReturnSortedBranches()
    {
        var branches = new List<GitBranch>
        {
            GitBranch.Default().SetAheadBehind(new AheadBehind(1, 0)),
            GitBranch.Default().SetAheadBehind(new AheadBehind(2, 0)),
            GitBranch.Default().SetAheadBehind(new AheadBehind(3, 0)),
        };

        var sortByAheadOption = new SortByAheadOptions();

        var result = sortByAheadOption.Execute(branches);

        Assert.Equal(3, result[0].AheadBehind.Ahead);
        Assert.Equal(2, result[1].AheadBehind.Ahead);
        Assert.Equal(1, result[2].AheadBehind.Ahead);
    }

    [Fact]
    public void SortByAheadOption_ShouldReturnSortedBranches_WhenBranchesAreAlreadySorted()
    {
        var branches = new List<GitBranch>
        {
            GitBranch.Default().SetAheadBehind(new AheadBehind(3, 0)),
            GitBranch.Default().SetAheadBehind(new AheadBehind(2, 0)),
            GitBranch.Default().SetAheadBehind(new AheadBehind(1, 0)),
        };

        var sortByAheadOption = new SortByAheadOptions();

        var result = sortByAheadOption.Execute(branches);

        Assert.Equal(3, result[0].AheadBehind.Ahead);
        Assert.Equal(2, result[1].AheadBehind.Ahead);
        Assert.Equal(1, result[2].AheadBehind.Ahead);
    }
}