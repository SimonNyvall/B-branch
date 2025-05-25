using Bbranch.GitService.Base;
using Bbranch.GitService.OptionStrategies.Common.BranchStrategies;
using Bbranch.Shared.TableData;

namespace Bbranch.Tests.GitService.Common.BranchStrategies;

public class BranchAllOptionTests
{
    [Fact]
    public void Given_BranchAllOptions_When_ExecuteRun_Then_Return_AllBranches()
    {
        var localBranches = new List<GitBranch>
        {
            GitBranch.Default().SetBranch(new Branch("main", isWorkingBranch: true)),
            GitBranch.Default().SetBranch(new Branch("feature/branch", isWorkingBranch: false))
        };

        var remoteBranches = new List<GitBranch>
        {
            GitBranch.Default().SetBranch(new Branch("origin/main", isWorkingBranch: false)),
            GitBranch.Default().SetBranch(new Branch("origin/feature/branch", isWorkingBranch: false))
        };

        IGitRepository mockGitBase = new GitRepositoryMock(localBranches, remoteBranches);
        var branchAllOptions = new BranchAllOptions(mockGitBase);

        List<GitBranch> result = branchAllOptions.Execute(new List<GitBranch>());

        Assert.Equal(4, result.Count);
    }

    [Fact]
    public void Given_BranchAllOptions_When_ExecuteRun_Then_Return_EmptyList_IfNoBranches()
    {
        IGitRepository gitBase = new GitRepositoryMock([], []);
        var branchAllOptions = new BranchAllOptions(gitBase);

        List<GitBranch> result = branchAllOptions.Execute([]);

        Assert.Empty(result);
    }

    private sealed class GitRepositoryMock(List<GitBranch> localValue, List<GitBranch> remoteValue) : IGitRepository
    {
        public List<GitBranch> GetLocalBranchNames()
        {
            return localValue;
        }

        public List<GitBranch> GetRemoteBranchNames()
        {
            return remoteValue;
        }
        
        public string GetWorkingBranch() => throw new NotImplementedException();
        public List<GitBranch> GetBranchDescription(List<GitBranch> branches) => throw new NotImplementedException();
        public AheadBehind GetLocalAheadBehind(string localBranchName) => throw new NotImplementedException();
        public AheadBehind GetRemoteAheadBehind(string localBranchName, string remoteBranchName) => throw new NotImplementedException();
        public DateTime GetLastCommitDate(string branchName) => throw new NotImplementedException();
        public bool DoesBranchExist(string branchName) => throw new NotImplementedException();
    }
}