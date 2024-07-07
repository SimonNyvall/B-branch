using Git.Options;
using Shared.TableData;

namespace Tests.GitService;

public class SortByBehindOptionTests
{
    [Fact]
    public void SortByBehindOption_ShouldReturnSortedBranches()
    {
        var branches = new List<GitBranch>
        {
            GitBranch.Default().SetAheadBehind(new AheadBehind(0, 1)),
            GitBranch.Default().SetAheadBehind(new AheadBehind(0, 2)),
            GitBranch.Default().SetAheadBehind(new AheadBehind(0, 3)),
        };

        var sortByBehindOption = new SortByBehindOptions();

        var result = sortByBehindOption.Execute(branches);

        Assert.Equal(3, result[0].AheadBehind.Behind);
        Assert.Equal(2, result[1].AheadBehind.Behind);
        Assert.Equal(1, result[2].AheadBehind.Behind);
    }

    [Fact]
    public void SortByBehindOption_ShouldReturnSortedBranches_WhenBranchesAreAlreadySorted()
    {
        var branches = new List<GitBranch>
        {
            GitBranch.Default().SetAheadBehind(new AheadBehind(0, 3)),
            GitBranch.Default().SetAheadBehind(new AheadBehind(0, 2)),
            GitBranch.Default().SetAheadBehind(new AheadBehind(0, 1)),
        };

        var sortByBehindOption = new SortByBehindOptions();

        var result = sortByBehindOption.Execute(branches);

        Assert.Equal(3, result[0].AheadBehind.Behind);
        Assert.Equal(2, result[1].AheadBehind.Behind);
        Assert.Equal(1, result[2].AheadBehind.Behind);
    }
}