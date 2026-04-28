using Bbranch.GitService.OptionStrategies.Common.SortStrategies;
using Bbranch.Shared.TableData;

namespace Bbranch.Tests.GitService.Common.SortStrategies;

[Trait("Category", "Unit")]
public sealed class SortByDetachedHeadOptionTests
{
    [Fact]
    public void Given_SortByDetachedHeadOptions_When_ExecuteRun_Then_Return_SortedBranches()
    {
        var detachedHeadName = "(HEAD detached at 6efb99e)";
        var branches = new HashSet<GitBranch>
        {
            GitBranch.Default().SetBranch(new Branch("feature", false)),
            GitBranch.Default().SetBranch(new Branch("main", false)),
            GitBranch
                .Default()
                .SetBranch(new Branch(detachedHeadName, true))
                .SetDetachedHead("123456"),
        };

        var sortByNameOption = new SortByDetachedHeadOption();

        var result = sortByNameOption.Execute(branches);

        Assert.Equal(detachedHeadName, result.First().Branch.Name);
        Assert.Equal("feature", result.ElementAt(1).Branch.Name);
        Assert.Equal("main", result.ElementAt(2).Branch.Name);
    }

    [Fact]
    public void Given_SortByDetachedHeadOptions_When_ExecuteRun_Then_Return_SortedBranches_WhenAlreadySorted()
    {
        var detachedHeadName = "(HEAD detached at 6efb99e)";
        var branches = new HashSet<GitBranch>
        {
            GitBranch
                .Default()
                .SetBranch(new Branch(detachedHeadName, true))
                .SetDetachedHead("123456"),
            GitBranch.Default().SetBranch(new Branch("b_feature", false)),
            GitBranch.Default().SetBranch(new Branch("c_feature", false)),
        };

        var sortByNameOption = new SortByNameOptions();

        var result = sortByNameOption.Execute(branches);

        Assert.Equal(detachedHeadName, result.First().Branch.Name);
        Assert.Equal("b_feature", result.ElementAt(1).Branch.Name);
        Assert.Equal("c_feature", result.ElementAt(2).Branch.Name);
    }
}
