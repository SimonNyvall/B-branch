using Bbranch.GitService.OptionStrategies.Common.SortStrategies;
using Bbranch.Shared.TableData;

namespace Bbranch.Tests.GitService.Common.SortStrategies;

public sealed class SortByBehindOptionTests
{
    [Fact]
    public void Given_SortByBehindOptions_When_ExecuteRun_Then_Return_SortedBranches()
    {
        var branches = new HashSet<GitBranch>
        {
            GitBranch.Default().SetAheadBehind(new AheadBehind(0, 1)),
            GitBranch.Default().SetAheadBehind(new AheadBehind(0, 2)),
            GitBranch.Default().SetAheadBehind(new AheadBehind(0, 3)),
        };

        var sortByBehindOption = new SortByBehindOptions();

        var result = sortByBehindOption.Execute(branches);

        Assert.Equal(3, result.First().AheadBehind.Behind);
        Assert.Equal(2, result.ElementAt(1).AheadBehind.Behind);
        Assert.Equal(1, result.ElementAt(2).AheadBehind.Behind);
    }

    [Fact]
    public void Given_SortByBehindOptions_When_ExecuteRun_Then_Return_SortedBranches_WhenAlreadySorted()
    {
        var branches = new HashSet<GitBranch>
        {
            GitBranch.Default().SetAheadBehind(new AheadBehind(0, 3)),
            GitBranch.Default().SetAheadBehind(new AheadBehind(0, 2)),
            GitBranch.Default().SetAheadBehind(new AheadBehind(0, 1)),
        };

        var sortByBehindOption = new SortByBehindOptions();

        var result = sortByBehindOption.Execute(branches);

        Assert.Equal(3, result.First().AheadBehind.Behind);
        Assert.Equal(2, result.ElementAt(1).AheadBehind.Behind);
        Assert.Equal(1, result.ElementAt(2).AheadBehind.Behind);
    }
}