using Bbranch.GitService.OptionStrategies.Common.ContainsStrategies;
using Bbranch.Shared.TableData;

namespace Bbranch.Tests.GitService.Common.ContainsStrategies;

public sealed class ContainsOptionTests
{
    [Fact]
    public void Given_ContainsOption_When_ExecuteRun_Then_Return_Branch_IfNameContainsPattern()
    {
        var branches = new HashSet<GitBranch>
        {
            GitBranch.Default().SetBranch(new Branch("main", true)),
            GitBranch.Default().SetBranch(new Branch("feature", true)),
            GitBranch.Default().SetBranch(new Branch("branch", true)),    
        };

        var option = new ContainsOption("branch");

        var result = option.Execute(branches);

        Assert.Single(result);
        Assert.Equal("branch", result.First().Branch.Name);
    }

    [Fact]
    public void Given_ContainsOption_When_ExecuteRun_Then_Return_AllBranches_IfNamesContainPattern()
    {
        var branches = new HashSet<GitBranch>
        {
            GitBranch.Default().SetBranch(new Branch("main", true)),
            GitBranch.Default().SetBranch(new Branch("feature", true)),
            GitBranch.Default().SetBranch(new Branch("branch", true)),    
        };

        var option = new ContainsOption("main;feature;branch");

        var result = option.Execute(branches);

        Assert.Equal(3, result.Count);
    }
}