using Bbranch.GitService.Base;
using Bbranch.GitService.OptionStrategies.Common.BranchStrategies;
using Bbranch.Shared.TableData;

namespace Bbranch.Tests.GitService.Common.BranchStrategies;

public sealed class BranchLocalOptionTests
{
    [Fact]
    public void Given_BranchLocalOptions_When_ExecuteRun_Then_Return_LocalBranches()
    {
        var localBranches = new List<GitBranch>
        {
            GitBranch.Default().SetBranch(new Branch("main", isWorkingBranch: true)),
            GitBranch.Default().SetBranch(new Branch("feature/branch", isWorkingBranch: true))
        };
        
        IGitRepository gitBase = new GitRepositoryMock(localBranches);

        var branchLocalOptions = new BranchLocalOptions(gitBase);

        List<GitBranch> result = branchLocalOptions.Execute([]);

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void Given_BranchLocalOptions_When_ExecuteRun_Then_Return_EmptyList_IfNoLocalBranches()
    {
        var localBranches = new List<GitBranch>();

        IGitRepository gitBase = new GitRepositoryMock(localBranches);
        
        var branchLocalOptions = new BranchLocalOptions(gitBase);

        List<GitBranch> result = branchLocalOptions.Execute([]);

        Assert.Empty(result);
    }

    private sealed class GitRepositoryMock(List<GitBranch> returnValue) : IGitRepository
    {
        public List<GitBranch> GetLocalBranchNames()
        {
            return returnValue;
        }
        
        public string GetWorkingBranch() => throw new NotImplementedException();
        public List<GitBranch> GetRemoteBranchNames() => throw new NotImplementedException();
        public List<GitBranch> GetBranchDescription(List<GitBranch> branches) => throw new NotImplementedException();
        public AheadBehind GetLocalAheadBehind(string localBranchName) => throw new NotImplementedException();
        public AheadBehind GetRemoteAheadBehind(string localBranchName, string remoteBranchName) => throw new NotImplementedException();
        public DateTime GetLastCommitDate(string branchName) => throw new NotImplementedException();
        public bool DoesBranchExist(string branchName) => throw new NotImplementedException();
    }
}