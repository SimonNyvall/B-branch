using Bbranch.GitService.Base;
using Bbranch.GitService.OptionStrategies.Shared.Setters;
using Bbranch.Shared.TableData;
using FakeItEasy;

namespace Bbranch.Tests.GitService.Shared.Setters;

[Trait("Category", "Unit")]
public sealed class SetLastCommitTests
{
    private readonly IGitRepository _gitRepositoryFake;

    public SetLastCommitTests()
    {
        _gitRepositoryFake = A.Fake<IGitRepository>();
        A.CallTo(() => _gitRepositoryFake.GetLastCommitDate(A<string>._)).Returns(DateTime.Now);
    }

    [Fact]
    public void Given_SetLastCommitOptions_When_ExecuteRun_Then_Return_EmptyList()
    {
        var strategy = new SetLastCommitOptions(_gitRepositoryFake);

        var result = strategy.Execute([]);

        Assert.Empty(result);
    }

    [Fact]
    public void Given_SetLastCommitOptions_When_ExecuteRun_Then_Return_ExpectedValue()
    {
        var strategy = new SetLastCommitOptions(_gitRepositoryFake);

        var branches = new HashSet<GitBranch> { GitBranch.Default(), GitBranch.Default() };

        var result = strategy.Execute(branches);

        Assert.Equal(branches.Count, result.Count);
    }

    [Fact]
    public void Given_SetLastCommitOptions_When_ExecuteRunWithDetachedHead_Then_Return_ExpectedValue()
    {
        var commitHash = "6efb99e";
        var detachedBranchName = $"(HEAD detached at {commitHash})";

        var capturedBranchName = string.Empty;
        A.CallTo(() => _gitRepositoryFake.GetLastCommitDate(A<string>._))
            .Invokes((string branchName) => capturedBranchName = branchName)
            .Returns(DateTime.Now);

        var strategy = new SetLastCommitOptions(_gitRepositoryFake);

        var gitBranch = GitBranch
            .Default()
            .SetBranch(new Branch(detachedBranchName, true))
            .SetDetachedHead(commitHash);

        var branches = new HashSet<GitBranch> { gitBranch };

        var result = strategy.Execute(branches);

        Assert.Equal(branches.Count, result.Count);
        Assert.Equal(commitHash, capturedBranchName);
    }

    [Fact]
    public void Given_SetLastCommitOptions_When_ExecutingRunWithSymbolicBranch_Then_Return_ExpectedValue()
    {
        var capturedBranchName = string.Empty;
        A.CallTo(() => _gitRepositoryFake.GetLastCommitDate(A<string>._))
            .Invokes((string branchName) => capturedBranchName = branchName)
            .Returns(DateTime.Now);

        var strategy = new SetLastCommitOptions(_gitRepositoryFake);

        var refBranch = "origin/HEAD";
        var targetBranch = "origin/main";

        var gitBranch = GitBranch
            .Default()
            .SetBranch(new Branch($"{refBranch} -> {targetBranch}", false))
            .SetIsRemote(true)
            .SetSymLink(new Symbolic(refBranch, targetBranch));

        var branches = new HashSet<GitBranch> { gitBranch };

        var result = strategy.Execute(branches);

        Assert.Equal(branches.Count, result.Count);
        Assert.Equal(targetBranch, capturedBranchName);
    }
}
