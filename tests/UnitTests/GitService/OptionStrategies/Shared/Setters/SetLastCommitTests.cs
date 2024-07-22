using Git.Base;
using Git.Options;
using Shared.TableData;
using NSubstitute;

namespace Tests.GitService;

public class SetLastCommitTests
{
    [Fact]
    public void Execute_WithNoBranches_ReturnsEmptyList()
    {
        var gitBase = Substitute.For<IGitBase>();
        var strategy = new SetLastCommitOptions(gitBase);

        var result = strategy.Execute([]);

        Assert.Empty(result);
    }

    [Fact]
    public void Execute_WithBranches_ReturnsEcpectedValue()
    {
        var gitBase = Substitute.For<IGitBase>();
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