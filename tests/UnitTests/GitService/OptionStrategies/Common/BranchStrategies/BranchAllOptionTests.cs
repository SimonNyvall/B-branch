using Bbranch.GitService.Base;
using Bbranch.GitService.OptionStrategies.Common.BranchStrategies;
using Bbranch.Shared.TableData;

namespace Bbranch.Tests.GitService.Common.BranchStrategies;

public class BranchAllOptionTests
{
    [Fact]
    public void Given_BranchAllOptions_When_ExecuteRun_Then_Return_AllBranches()
    {
        var localBranches = new HashSet<GitBranch>
        {
            GitBranch.Default().SetBranch(new Branch("main", true)),
            GitBranch.Default().SetBranch(new Branch("feature/branch", false))
        };

        var remoteBranches = new HashSet<GitBranch>
        {
            GitBranch.Default().SetBranch(new Branch("origin/main", false)),
            GitBranch.Default().SetBranch(new Branch("origin/feature/branch", false))
        };

        IGitRepository mockGitBase = new GitRepositoryMock(localBranches, remoteBranches);
        var branchAllOptions = new BranchAllOptions(mockGitBase);

        HashSet<GitBranch> result = branchAllOptions.Execute([]);

        Assert.Equal(4, result.Count);
    }

    [Fact]
    public void Given_BranchAllOptions_When_ExecuteRun_Then_Return_EmptyList_IfNoBranches()
    {
        IGitRepository gitBase = new GitRepositoryMock([], []);
        var branchAllOptions = new BranchAllOptions(gitBase);

        HashSet<GitBranch> result = branchAllOptions.Execute([]);

        Assert.Empty(result);
    }

    private sealed class GitRepositoryMock(HashSet<GitBranch> localValue, HashSet<GitBranch> remoteValue) : IGitRepository
    {
        public HashSet<GitBranch> GetLocalBranchNames()
        {
            return localValue;
        }

        public HashSet<GitBranch> GetRemoteBranchNames()
        {
            return remoteValue;
        }
        
        public string GetWorkingBranch() => throw new NotImplementedException();
        public HashSet<GitBranch> GetBranchDescription(HashSet<GitBranch> branches) => throw new NotImplementedException();
        public Task<AheadBehind> GetLocalAheadBehind(string localBranchName) => throw new NotImplementedException();
        public Task<AheadBehind> GetRemoteAheadBehind(string localBranchName, string remoteBranchName) => throw new NotImplementedException();
        public DateTime GetLastCommitDate(string branchName) => throw new NotImplementedException();
    }
}