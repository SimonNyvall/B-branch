using Bbranch.GitService.OptionStrategies.Common.ContainsStrategies;
using Bbranch.Shared.TableData;

namespace Bbranch.Tests.GitService.Common.ContainsStrategies;

public class NoContainsOptionTests
{
    [Fact]
    public void Execute_WhenNoContainsOption_ShouldExcludeMain()
    {
        var branches = new List<GitBranch>
        {
            GitBranch.Default().SetBranch(new Branch("main", isWorkingBranch: true)),
            GitBranch.Default().SetBranch(new Branch("feature", isWorkingBranch: true)),
        };

        var noContainsOption = new NoContainsOption("main");

        List<GitBranch> result = noContainsOption.Execute(branches);

        Assert.Single(result);
        Assert.Equal("feature", result[0].Branch.Name);
    }

    [Fact]
    public void Execute_WhenNoContainsOption_ShouldReturnAllBranches()
    {
        var branches = new List<GitBranch>
        {
            GitBranch.Default().SetBranch(new Branch("main", isWorkingBranch: true)),
        };

        var noContainsOption = new NoContainsOption("feature");

        List<GitBranch> result = noContainsOption.Execute(branches);

        Assert.Single(result);
        Assert.Equal("main", result[0].Branch.Name);
    }

    [Fact]
    public void Execute_WhenNoContainsOption_ShouldExcludeMainForAllBranches()
    {
        var branches = new List<GitBranch>
        {
            GitBranch.Default().SetBranch(new Branch("main", isWorkingBranch: true)),
            GitBranch.Default().SetBranch(new Branch("origin/main", isWorkingBranch: true)),
            GitBranch.Default().SetBranch(new Branch("feature", isWorkingBranch: true)),
        };

        var noContainsOption = new NoContainsOption("main");

        List<GitBranch> result = noContainsOption.Execute(branches);

        Assert.Single(result);
        Assert.Equal("feature", result[0].Branch.Name);
    }
}
