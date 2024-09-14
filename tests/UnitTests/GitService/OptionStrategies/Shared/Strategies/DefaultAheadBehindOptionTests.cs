using Git.Base;
using Git.Options;
using Shared.TableData;
using NSubstitute;

namespace Tests.GitService;

public class DefaultAheadBehindOptionTests
{
    [Fact]
    public void Execute_WithNoBranches_ReturnsEmptyList()
    {
        var gitBase = Substitute.For<IGitRepository>();
        var strategy = new DefaultAheadBehindOption(gitBase);

        var result = strategy.Execute([]);

        Assert.Empty(result);
    }

    [Fact]
    public void Execute_WithBranches_ReturnsEcpectedValue()
    {
        var gitBase = Substitute.For<IGitRepository>();
        gitBase.GetLocalAheadBehind("").ReturnsForAnyArgs(new AheadBehind(1, 1));

        var strategy = new DefaultAheadBehindOption(gitBase);

        var branches = new List<GitBranch> 
        {
            GitBranch.Default(),
            GitBranch.Default()
        };

        var result = strategy.Execute(branches);

        Assert.Equal(branches.Count, result.Count);
    }
}