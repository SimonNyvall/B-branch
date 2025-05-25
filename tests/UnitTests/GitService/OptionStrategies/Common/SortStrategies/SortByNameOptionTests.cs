using Bbranch.GitService.OptionStrategies.Common.SortStrategies;
using Bbranch.Shared.TableData;

namespace Bbranch.Tests.GitService.Common.SortStrategies;

public sealed class SortByNameOptionTests
{
    [Fact]
    public void Given_SortByNameOptions_When_ExecuteRun_Then_Return_SortedBranches()
    {
        var branches = new List<GitBranch>
        {
            GitBranch.Default().SetBranch(new Branch("b_feature", isWorkingBranch: false)),
            GitBranch.Default().SetBranch(new Branch("a_main", isWorkingBranch: true)),
            GitBranch.Default().SetBranch(new Branch("c_feature", isWorkingBranch: false)),
        };

        var sortByNameOption = new SortByNameOptions();

        var result = sortByNameOption.Execute(branches);

        Assert.Equal("a_main", result[0].Branch.Name);
        Assert.Equal("b_feature", result[1].Branch.Name);
        Assert.Equal("c_feature", result[2].Branch.Name);
    }

    [Fact]
    public void Given_SortByNameOptions_When_ExecuteRun_Then_Return_SortedBranches_WhenAlreadySorted()
    {
        var branches = new List<GitBranch>
        {
            GitBranch.Default().SetBranch(new Branch("a_main", isWorkingBranch: true)),
            GitBranch.Default().SetBranch(new Branch("b_feature", isWorkingBranch: false)),
            GitBranch.Default().SetBranch(new Branch("c_feature", isWorkingBranch: false)),
        };

        var sortByNameOption = new SortByNameOptions();

        var result = sortByNameOption.Execute(branches);

        Assert.Equal("a_main", result[0].Branch.Name);
        Assert.Equal("b_feature", result[1].Branch.Name);
        Assert.Equal("c_feature", result[2].Branch.Name);
    }
}