using Bbranch.GitService.Base;
using Bbranch.GitService.OptionStrategies.Common.BranchStrategies;
using Bbranch.Shared.TableData;

namespace Bbranch.Tests.GitService.Common.BranchStrategies;

public sealed class BranchRemoteOptionTests
{
    [Fact]
    public void Given_BranchRemoteOptions_When_ExecuteRun_Then_Return_RemoteBranches()
    {
        var remoteBranches = new HashSet<GitBranch>
        {
            GitBranch.Default().SetBranch(new Branch("origin/main", false)),
            GitBranch.Default().SetBranch(new Branch("origin/feature/branch", false))
        };
        
        IGitRepository mockGitBase = new GitRepositoryMock(remoteBranches);
        var branchRemoteOptions = new BranchRemoteOptions(mockGitBase);

        HashSet<GitBranch> result = branchRemoteOptions.Execute([]);

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void Given_BranchRemoteOptions_When_ExecuteRun_Then_Return_EmptyList_IfNoRemoteBranches()
    {
        IGitRepository mockGitBase = new GitRepositoryMock([]);
        var branchRemoteOptions = new BranchRemoteOptions(mockGitBase);

        HashSet<GitBranch> result = branchRemoteOptions.Execute([]);

        Assert.Empty(result);
    }

    private sealed class GitRepositoryMock(HashSet<GitBranch> value) : IGitRepository
    {
        public HashSet<GitBranch> GetRemoteBranchNames()
        {
            return value;
        }
        
        public string GetWorkingBranch() => throw new NotImplementedException();
        public HashSet<GitBranch> GetLocalBranchNames() => throw new NotImplementedException();
        public HashSet<GitBranch> GetBranchDescription(HashSet<GitBranch> branches) => throw new NotImplementedException();
        public Task<AheadBehind> GetLocalAheadBehind(string localBranchName) => throw new NotImplementedException();
        public Task<AheadBehind> GetRemoteAheadBehind(string localBranchName, string remoteBranchName) => throw new NotImplementedException();
        public DateTime GetLastCommitDate(string branchName) => throw new NotImplementedException();
    }
}