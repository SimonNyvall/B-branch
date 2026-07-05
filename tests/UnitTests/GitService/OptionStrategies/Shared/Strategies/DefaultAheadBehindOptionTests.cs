using Bbranch.GitService.Base;
using Bbranch.GitService.OptionStrategies.Shared.Strategies;
using Bbranch.Shared.TableData;
using FakeItEasy;

namespace Bbranch.Tests.GitService.Shared.Strategies;

[Trait("Category", "Unit")]
public sealed class DefaultAheadBehindOptionTests
{
    private readonly IGitRepository _gitRepositoryFake;
    private readonly List<GitBranch> _gitBranches;

    public DefaultAheadBehindOptionTests()
    {
        _gitBranches =
        [
            GitBranch.Default().SetAheadBehind(new AheadBehind(1, 1)),
            GitBranch.Default().SetAheadBehind(new AheadBehind(1, 1)),
        ];

        _gitRepositoryFake = A.Fake<IGitRepository>();
        A.CallTo(() => _gitRepositoryFake.GetLocalAheadBehind(A<GitBranch>._))
            .Returns(_gitBranches.First());
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

        var result = await strategy.Execute(_gitBranches);

        Assert.Equal(_gitBranches.Count, result.Count);
    }
}
