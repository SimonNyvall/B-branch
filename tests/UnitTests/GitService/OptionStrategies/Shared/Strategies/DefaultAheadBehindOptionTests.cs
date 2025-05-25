using Bbranch.GitService.Base;
using Bbranch.GitService.OptionStrategies.Shared.Strategies;
using Bbranch.Shared.TableData;

namespace Bbranch.Tests.GitService.Shared.Strategies;

public sealed class DefaultAheadBehindOptionTests
{
    private readonly IGitRepository _gitBase = new GitRepositoryMock();
    
    [Fact]
    public void Given_DefaultAheadBehindOption_When_ExecuteRun_Then_Return_EmptyList()
    {
        var strategy = new DefaultAheadBehindOption(_gitBase);

        var result = strategy.Execute([]);

        Assert.Empty(result);
    }

    [Fact]
    public void Given_DefaultAheadBehindOption_When_ExecuteRun_Then_Return_ExpectedValue()
    {
        var strategy = new DefaultAheadBehindOption(_gitBase);

        var branches = new List<GitBranch> 
        {
            GitBranch.Default(),
            GitBranch.Default()
        };

        var result = strategy.Execute(branches);

        Assert.Equal(branches.Count, result.Count);
    }

    private sealed class GitRepositoryMock : IGitRepository
    {
        public AheadBehind GetLocalAheadBehind(string localBranchName)
        {
            return new AheadBehind(1, 1);
        }
        
        public List<GitBranch> GetLocalBranchNames() => throw new NotImplementedException();
        public string GetWorkingBranch() => throw new NotImplementedException();
        public List<GitBranch> GetRemoteBranchNames() => throw new NotImplementedException();
        public List<GitBranch> GetBranchDescription(List<GitBranch> branches) => throw new NotImplementedException();
        public AheadBehind GetRemoteAheadBehind(string localBranchName, string remoteBranchName) => throw new NotImplementedException();
        public DateTime GetLastCommitDate(string branchName) => throw new NotImplementedException();
        public bool DoesBranchExist(string branchName) => throw new NotImplementedException();
    }
}