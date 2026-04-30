using Bbranch.GitService.OptionStrategies;
using Bbranch.Shared.TableData;
using FakeItEasy;

namespace Bbranch.Tests.GitService.OptionStrategies;

[Trait("Category", "Unit")]
public sealed class OptionStrategyTests
{
    [Fact]
    public async Task Given_CompositeOptionStrategy_When_ExecuteRun_Then_Return_OriginalBranches()
    {
        var originalBranches = new HashSet<GitBranch> { GitBranch.Default(), GitBranch.Default() };

        var strategy = new CompositeOptionStrategy([]);

        var result = await strategy.Execute(originalBranches);

        Assert.Equal(originalBranches.Count, result.Count);
    }

    [Fact]
    public async Task Given_CompositeOptionStrategy_When_ExecuteRun_Then_Return_ModifiedBranches()
    {
        var originalBranches = new HashSet<GitBranch> { GitBranch.Default(), GitBranch.Default() };
        var modifiedBranches = new HashSet<GitBranch> { GitBranch.Default() };

        var optionFake = A.Fake<IOption>();
        A.CallTo(() => optionFake.Execute(A<HashSet<GitBranch>>._)).Returns(modifiedBranches);
        var strategy = new CompositeOptionStrategy([optionFake]);

        var result = await strategy.Execute(originalBranches);

        Assert.Equal(modifiedBranches.Count, result.Count);
    }

    [Fact]
    public void Given_CompositeOptionStrategy_When_AddStrategyOptionRun_Then_Set_NewStrategy()
    {
        var strategy = new CompositeOptionStrategy([]);

        var optionFake = A.Fake<IOption>();

        strategy.AddStrategyOption(optionFake);
        var result = strategy.Execute([]);

        Assert.NotNull(result);
    }
}
