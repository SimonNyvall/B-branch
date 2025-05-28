using Bbranch.GitService.Base;
using Bbranch.GitService.OptionStrategies.Common.BranchStrategies;
using Bbranch.Shared.TableData;

namespace Bbranch.Tests.GitService.Common.BranchStrategies;

public sealed class BranchLocalOptionTests
{
    [Fact]
    public void Given_BranchLocalOptions_When_ExecuteRun_Then_Return_LocalBranches()
    {
        var localBranches = new HashSet<GitBranch>
        {
            GitBranch.Default().SetBranch(new Branch("main", true)),
            GitBranch.Default().SetBranch(new Branch("feature/branch", true))
        };
        
        IGitRepository gitBase = new GitRepositoryMock(localBranches);

        var branchLocalOptions = new BranchLocalOptions(gitBase);

        HashSet<GitBranch> result = branchLocalOptions.Execute([]);

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void Given_BranchLocalOptions_When_ExecuteRun_Then_Return_EmptyList_IfNoLocalBranches()
    {
        var localBranches = new HashSet<GitBranch>();

        IGitRepository gitBase = new GitRepositoryMock(localBranches);
        
        var branchLocalOptions = new BranchLocalOptions(gitBase);

        HashSet<GitBranch> result = branchLocalOptions.Execute([]);

        Assert.Empty(result);
    }

    private sealed class GitRepositoryMock(HashSet<GitBranch> returnValue) : IGitRepository
    {
        public HashSet<GitBranch> GetLocalBranchNames()
        {
            return returnValue;
        }
        
        public string GetWorkingBranch() => throw new NotImplementedException();
        public HashSet<GitBranch> GetRemoteBranchNames() => throw new NotImplementedException();
        public HashSet<GitBranch> GetBranchDescription(HashSet<GitBranch> branches) => throw new NotImplementedException();
        public Task<AheadBehind> GetLocalAheadBehind(string localBranchName) => throw new NotImplementedException();
        public Task<AheadBehind> GetRemoteAheadBehind(string localBranchName, string remoteBranchName) => throw new NotImplementedException();
        public DateTime GetLastCommitDate(string branchName) => throw new NotImplementedException();
    }
}