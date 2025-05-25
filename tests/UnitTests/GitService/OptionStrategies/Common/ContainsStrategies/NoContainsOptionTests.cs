using Bbranch.GitService.OptionStrategies.Common.ContainsStrategies;
using Bbranch.Shared.TableData;

namespace Bbranch.Tests.GitService.Common.ContainsStrategies;

public sealed class NoContainsOptionTests
{
    [Fact]
    public void Given_NoContainsOption_When_ExecuteRun_Then_Exclude_Main()
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
    public void Given_NoContainsOption_When_ExecuteRun_Then_Return_AllBranches()
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
    public void Given_NoContainsOption_When_ExecuteRun_Then_Exclude_Main_ForAllBranches()
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
