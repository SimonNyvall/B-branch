using Bbranch.GitService.Base;
using Bbranch.GitService.OptionStrategies.Shared.Strategies;
using Bbranch.Shared.TableData;
using NSubstitute;

namespace Bbranch.Tests.GitService.Shared.Strategies;

public class TraclAheadBehindOptionTests
{
    [Fact]
    public void Execute_WithNoBranches_ReturnsEmptyList()
    {
        var gitBase = Substitute.For<IGitRepository>();
        gitBase.GetRemoteAheadBehind("", "").ReturnsForAnyArgs(new AheadBehind(1, 1));

        var strategy = new TrackAheadBehindOption(gitBase, "");

        var result = strategy.Execute([]);

        Assert.Empty(result);
    }

    [Fact]
    public void Execute_WithBranches_ReturnsEcpectedValue()
    {
        var gitBase = Substitute.For<IGitRepository>();
        gitBase.GetRemoteAheadBehind("", "").ReturnsForAnyArgs(new AheadBehind(1, 1));

        var strategy = new TrackAheadBehindOption(gitBase, "");

        var branches = new List<GitBranch> 
        {
            GitBranch.Default(),
            GitBranch.Default()
        };

        var result = strategy.Execute(branches);

        Assert.Equal(branches.Count, result.Count);
    }
}