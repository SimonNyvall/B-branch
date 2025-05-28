using Bbranch.GitService.OptionStrategies.Common.ContainsStrategies;
using Bbranch.Shared.TableData;

namespace Bbranch.Tests.GitService.Common.ContainsStrategies;

public sealed class NoContainsOptionTests
{
    [Fact]
    public void Given_NoContainsOption_When_ExecuteRun_Then_Exclude_Main()
    {
        var branches = new HashSet<GitBranch>
        {
            GitBranch.Default().SetBranch(new Branch("main", true)),
            GitBranch.Default().SetBranch(new Branch("feature", true)),
        };

        var noContainsOption = new NoContainsOption("main");

        HashSet<GitBranch> result = noContainsOption.Execute(branches);

        Assert.Single(result);
        Assert.Equal("feature", result.First().Branch.Name);
    }

    [Fact]
    public void Given_NoContainsOption_When_ExecuteRun_Then_Return_AllBranches()
    {
        var branches = new HashSet<GitBranch>
        {
            GitBranch.Default().SetBranch(new Branch("main", true)),
        };

        var noContainsOption = new NoContainsOption("feature");

        HashSet<GitBranch> result = noContainsOption.Execute(branches);

        Assert.Single(result);
        Assert.Equal("main", result.First().Branch.Name);
    }

    [Fact]
    public void Given_NoContainsOption_When_ExecuteRun_Then_Exclude_Main_ForAllBranches()
    {
        var branches = new HashSet<GitBranch>
        {
            GitBranch.Default().SetBranch(new Branch("main", true)),
            GitBranch.Default().SetBranch(new Branch("origin/main", true)),
            GitBranch.Default().SetBranch(new Branch("feature", true)),
        };

        var noContainsOption = new NoContainsOption("main");

        HashSet<GitBranch> result = noContainsOption.Execute(branches);

        Assert.Single(result);
        Assert.Equal("feature", result.First().Branch.Name);
    }
}
