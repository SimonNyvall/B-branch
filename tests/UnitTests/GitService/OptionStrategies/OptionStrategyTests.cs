using Bbranch.GitService.OptionStrategies;
using Bbranch.Shared.TableData;

namespace Bbranch.Tests.GitService.OptionStrategies;

public sealed class OptionStrategyTests
{
    [Fact]
    public void Given_CompositeOptionStrategy_When_ExecuteRun_Then_Return_OriginalBranches()
    {
        var originalBranches = new HashSet<GitBranch> 
        { 
            GitBranch.Default(),
            GitBranch.Default()
        };

        var strategy = new CompositeOptionStrategy(new List<IOption>());

        var result = strategy.Execute(originalBranches);

        Assert.Equal(originalBranches.Count, result.Count);
    }

    [Fact]
    public void Given_CompositeOptionStrategy_When_ExecuteRun_Then_Return_ModifiedBranches()
    {
        var originalBranches = new HashSet<GitBranch> 
        {
            GitBranch.Default(),
            GitBranch.Default()
        };
        var modifiedBranches = new HashSet<GitBranch>
        {
            GitBranch.Default()
        };

        var mockStrategy = new MockOption(modifiedBranches);
        var strategy = new CompositeOptionStrategy([mockStrategy]);

        var result = strategy.Execute(originalBranches);

        Assert.Equal(modifiedBranches.Count, result.Count);
    }

    [Fact]
    public void Given_CompositeOptionStrategy_When_AddStrategyOptionRun_Then_Set_NewStrategy()
    {
        var strategy = new CompositeOptionStrategy([]);
        var mockStrategy = new MockOption([]);

        strategy.AddStrategyOption(mockStrategy);
        var result = strategy.Execute([]);

        Assert.NotNull(result);
    }

    private sealed class MockOption : IOption
    {
        private readonly HashSet<GitBranch> _branchesToReturn;

        public MockOption(HashSet<GitBranch> branchesToReturn)
        {
            _branchesToReturn = branchesToReturn;
        }

        public HashSet<GitBranch> Execute(HashSet<GitBranch> branches)
        {
            return _branchesToReturn;
        }
    }
}