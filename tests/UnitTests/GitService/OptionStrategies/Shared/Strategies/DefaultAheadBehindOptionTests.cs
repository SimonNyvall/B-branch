using Bbranch.GitService.Base;
using Bbranch.GitService.OptionStrategies.Shared.Strategies;
using Bbranch.Shared.TableData;
using FakeItEasy;

namespace Bbranch.Tests.GitService.Shared.Strategies;

[Trait("Category", "Unit")]
public sealed class DefaultAheadBehindOptionTests
{
    private readonly IGitRepository _gitRepositoryFake;

    public DefaultAheadBehindOptionTests()
    {
        _gitRepositoryFake = A.Fake<IGitRepository>();
        A.CallTo(() => _gitRepositoryFake.GetLocalAheadBehind(A<string>._))
            .Returns(new AheadBehind(1, 1));
    }

    [Fact]
    public async Task Given_DefaultAheadBehindOption_When_ExecuteRun_Then_Return_EmptyList()
    {
        var strategy = new DefaultAheadBehindOption(_gitRepositoryFake);

        var result = await strategy.Execute([]);

        Assert.Empty(result);
    }

    [Fact]
    public async Task Given_DefaultAheadBehindOption_When_ExecuteRun_Then_Return_ExpectedValue()
    {
        var strategy = new DefaultAheadBehindOption(_gitRepositoryFake);

        var branches = new HashSet<GitBranch> { GitBranch.Default(), GitBranch.Default() };

        var result = await strategy.Execute(branches);

        Assert.Equal(branches.Count, result.Count);
    }
}
