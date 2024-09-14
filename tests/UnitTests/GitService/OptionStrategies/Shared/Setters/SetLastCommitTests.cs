using Bbranch.GitService.Base;
using Bbranch.GitService.OptionStrategies.Shared.Setters;
using Bbranch.Shared.TableData;
using NSubstitute;

namespace Bbranch.Tests.GitService.Shared.Setters;

public class SetLastCommitTests
{
    [Fact]
    public void Execute_WithNoBranches_ReturnsEmptyList()
    {
        var gitBase = Substitute.For<IGitRepository>();
        var strategy = new SetLastCommitOptions(gitBase);

        var result = strategy.Execute([]);

        Assert.Empty(result);
    }

    [Fact]
    public void Execute_WithBranches_ReturnsEcpectedValue()
    {
        var gitBase = Substitute.For<IGitRepository>();
        gitBase.GetLastCommitDate("").ReturnsForAnyArgs(DateTime.Now);

        var strategy = new SetLastCommitOptions(gitBase);

        var branches = new List<GitBranch> 
        {
            GitBranch.Default(),
            GitBranch.Default()
        };

        var result = strategy.Execute(branches);

        Assert.Equal(branches.Count, result.Count);
    }
}