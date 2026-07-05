using Bbranch.GitService.OptionStrategies.Common.SortStrategies;
using Bbranch.Shared.TableData;

namespace Bbranch.Tests.GitService.Common.SortStrategies;

[Trait("Category", "Unit")]
public sealed class SortByNameOptionTests
{
    [Fact]
    public async Task Given_SortByNameOptions_When_ExecuteRun_Then_Return_SortedBranches()
    {
        var branches = new List<GitBranch>
        {
            GitBranch.Default().SetBranch(new BranchViewModel("b_feature", false)),
            GitBranch.Default().SetBranch(new BranchViewModel("a_main", true)),
            GitBranch.Default().SetBranch(new BranchViewModel("c_feature", false)),
        };

        var sortByNameOption = new SortByNameOptions();

        var result = await sortByNameOption.Execute(branches);

        Assert.Equal("a_main", result.First().Branch.Name);
        Assert.Equal("b_feature", result.ElementAt(1).Branch.Name);
        Assert.Equal("c_feature", result.ElementAt(2).Branch.Name);
    }

    [Fact]
    public async Task Given_SortByNameOptions_When_ExecuteRun_Then_Return_SortedBranches_WhenAlreadySorted()
    {
        var branches = new List<GitBranch>
        {
            GitBranch.Default().SetBranch(new BranchViewModel("a_main", true)),
            GitBranch.Default().SetBranch(new BranchViewModel("b_feature", false)),
            GitBranch.Default().SetBranch(new BranchViewModel("c_feature", false)),
        };

        var sortByNameOption = new SortByNameOptions();

        var result = await sortByNameOption.Execute(branches);

        Assert.Equal("a_main", result.First().Branch.Name);
        Assert.Equal("b_feature", result.ElementAt(1).Branch.Name);
        Assert.Equal("c_feature", result.ElementAt(2).Branch.Name);
    }
}
