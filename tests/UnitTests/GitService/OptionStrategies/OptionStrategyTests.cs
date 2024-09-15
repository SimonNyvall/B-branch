using Bbranch.GitService.OptionStrategies;
using Bbranch.Shared.TableData;

namespace Bbranch.Tests.GitService.OptionStrategies;

public class OptionStrategyTests
{
    [Fact]
    public void Execute_WithNoStrategies_ReturnsOriginalBranches()
    {
        var originalBranches = new List<GitBranch> 
        { 
            GitBranch.Default(),
            GitBranch.Default()
        };

        var strategy = new CompositeOptionStrategy(new List<IOption>());

        var result = strategy.Execute(originalBranches);

        Assert.Equal(originalBranches.Count, result.Count);
    }

    [Fact]
    public void Execute_WithSingleStrategy_ModifiesBranchesAccordingly()
    {
        var originalBranches = new List<GitBranch> 
        {
            GitBranch.Default(),
            GitBranch.Default()
        };
        var modifiedBranches = new List<GitBranch> 
        {
            GitBranch.Default()
        };

        var mockStrategy = new MockOption(modifiedBranches);
        var strategy = new CompositeOptionStrategy([mockStrategy]);

        var result = strategy.Execute(originalBranches);

        Assert.Equal(modifiedBranches.Count, result.Count);
    }

    [Fact]
    public void AddStrategyOption_AddsNewStrategy()
    {
        var strategy = new CompositeOptionStrategy([]);
        var mockStrategy = new MockOption([]);

        strategy.AddStrategyOption(mockStrategy);
        var result = strategy.Execute([]);

        Assert.NotNull(result);
    }

    private class MockOption : IOption
    {
        private readonly List<GitBranch> _branchesToReturn;

        public MockOption(List<GitBranch> branchesToReturn)
        {
            _branchesToReturn = branchesToReturn;
        }

        public List<GitBranch> Execute(List<GitBranch> branches)
        {
            return _branchesToReturn;
        }
    }
}