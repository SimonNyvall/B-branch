using Bbranch.GitService.Base;
using Bbranch.GitService.OptionStrategies.Shared.Strategies;
using Bbranch.Shared.TableData;

namespace Bbranch.Tests.GitService.Shared.Strategies;

public sealed class TrackAheadBehindOptionTests
{
    private readonly IGitRepository _gitBase = new GitRepositoryMock();

    [Fact]
    public void Given_TrackAheadBehindOption_When_ExecuteRun_Then_Return_EmptyList()
    {
        var strategy = new TrackAheadBehindOption(_gitBase, "");

        var result = strategy.Execute([]);

        Assert.Empty(result);
    }

    [Fact]
    public void Given_TrackAheadBehindOption_When_ExecuteRun_Then_Return_ExpectedValue()
    {
        var strategy = new TrackAheadBehindOption(_gitBase, "");

        var branches = new HashSet<GitBranch> 
        {
            GitBranch.Default(),
            GitBranch.Default()
        };

        var result = strategy.Execute(branches);

        Assert.Equal(branches.Count, result.Count);
    }

    private sealed class GitRepositoryMock : IGitRepository
    {
        public Task<AheadBehind> GetRemoteAheadBehind(string localBranchName, string remoteBranchName)
        {
            return Task.FromResult(new AheadBehind(1, 1));
        }
        
        public string GetWorkingBranch() => throw new NotImplementedException();
        public HashSet<GitBranch> GetLocalBranchNames() => throw new NotImplementedException();
        public HashSet<GitBranch> GetRemoteBranchNames() => throw new NotImplementedException();
        public HashSet<GitBranch> GetBranchDescription(HashSet<GitBranch> branches) => throw new NotImplementedException();
        public Task<AheadBehind> GetLocalAheadBehind(string localBranchName) => throw new NotImplementedException();
        public DateTime GetLastCommitDate(string branchName) => throw new NotImplementedException();
    
    }
}