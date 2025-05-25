using Bbranch.GitService.OptionStrategies.Common.ContainsStrategies;
using Bbranch.Shared.TableData;

namespace Bbranch.Tests.GitService.Common.ContainsStrategies;

public sealed class ContainsOptionTests
{
    [Fact]
    public void Given_ContainsOption_When_ExecuteRun_Then_Return_Branch_IfNameContainsPattern()
    {
        var branches = new List<GitBranch>
        {
            GitBranch.Default().SetBranch(new Branch("main", isWorkingBranch: true)),
            GitBranch.Default().SetBranch(new Branch("feature", isWorkingBranch: true)),
            GitBranch.Default().SetBranch(new Branch("branch", isWorkingBranch: true)),    
        };

        var option = new ContainsOption("branch");

        var result = option.Execute(branches);

        Assert.Single(result);
        Assert.Equal("branch", result[0].Branch.Name);
    }

    [Fact]
    public void Given_ContainsOption_When_ExecuteRun_Then_Return_AllBranches_IfNamesContainPattern()
    {
        var branches = new List<GitBranch>
        {
            GitBranch.Default().SetBranch(new Branch("main", isWorkingBranch: true)),
            GitBranch.Default().SetBranch(new Branch("feature", isWorkingBranch: true)),
            GitBranch.Default().SetBranch(new Branch("branch", isWorkingBranch: true)),    
        };

        var option = new ContainsOption("main;feature;branch");

        var result = option.Execute(branches);

        Assert.Equal(3, result.Count);
    }
}