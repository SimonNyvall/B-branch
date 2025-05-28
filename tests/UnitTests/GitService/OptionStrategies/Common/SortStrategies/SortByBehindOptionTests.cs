using Bbranch.GitService.OptionStrategies.Common.SortStrategies;
using Bbranch.Shared.TableData;

namespace Bbranch.Tests.GitService.Common.SortStrategies;

public sealed class SortByAheadOptionTests
{
    [Fact]
    public void Given_SortByAheadOptions_When_ExecuteRun_Then_Return_SortedBranches()
    {
        var branches = new HashSet<GitBranch>
        {
            GitBranch.Default().SetAheadBehind(new AheadBehind(1, 0)),
            GitBranch.Default().SetAheadBehind(new AheadBehind(2, 0)),
            GitBranch.Default().SetAheadBehind(new AheadBehind(3, 0)),
        };

        var sortByAheadOption = new SortByAheadOptions();

        var result = sortByAheadOption.Execute(branches);

        Assert.Equal(3, result.First().AheadBehind.Ahead);
        Assert.Equal(2, result.ElementAt(1).AheadBehind.Ahead);
        Assert.Equal(1, result.ElementAt(2).AheadBehind.Ahead);
    }

    [Fact]
    public void Given_SortByAheadOptions_When_ExecuteRun_Then_Return_SortedBranches_WhenAlreadySorted()
    {
        var branches = new HashSet<GitBranch>
        {
            GitBranch.Default().SetAheadBehind(new AheadBehind(3, 0)),
            GitBranch.Default().SetAheadBehind(new AheadBehind(2, 0)),
            GitBranch.Default().SetAheadBehind(new AheadBehind(1, 0)),
        };

        var sortByAheadOption = new SortByAheadOptions();

        var result = sortByAheadOption.Execute(branches);

        Assert.Equal(3, result.First().AheadBehind.Ahead);
        Assert.Equal(2, result.ElementAt(1).AheadBehind.Ahead);
        Assert.Equal(1, result.ElementAt(2).AheadBehind.Ahead);
    }
}