using Bbranch.GitService.Base;
using Bbranch.GitService.OptionStrategies.Common;
using Bbranch.Shared.TableData;

namespace Tests.GitService;

public sealed class WorkingBranchOptionTests
{
    private readonly IGitRepository _gitBase = new GitRepositoryMock();
    
    [Fact]
    public void Given_WorkingBranchOption_When_ExecuteRun_Then_Return_WorkingBranch()
    {
        var branches = new HashSet<GitBranch>
        {
            GitBranch.Default().SetBranch(new Branch("main", false)),
            GitBranch.Default().SetBranch(new Branch("feature", false))
        };

        var workingBranchOption = new WorkingBranchOption(_gitBase);

        HashSet<GitBranch> result = workingBranchOption.Execute(branches);

        Assert.True(result.First().Branch.IsWorkingBranch);
        Assert.False(result.ElementAt(1).Branch.IsWorkingBranch);
    }

    [Fact]
    public void Given_WorkingBranchOption_When_ExecuteRun_Then_Return_EmptyList()
    {
        HashSet<GitBranch> branches = [];

        var workingBranchOption = new WorkingBranchOption(_gitBase);

        HashSet<GitBranch> result = workingBranchOption.Execute(branches);

        Assert.Empty(result);
    }
    
    private sealed class GitRepositoryMock : IGitRepository
    {
        public string GetWorkingBranch()
        {
            return "main";
        }

        public HashSet<GitBranch> GetLocalBranchNames() => throw new NotImplementedException();
        public HashSet<GitBranch> GetRemoteBranchNames() => throw new NotImplementedException();
        public HashSet<GitBranch> GetBranchDescription(HashSet<GitBranch> branches) => throw new NotImplementedException();
        public Task<AheadBehind> GetLocalAheadBehind(string localBranchName) => throw new NotImplementedException();
        public Task<AheadBehind> GetRemoteAheadBehind(string localBranchName, string remoteBranchName) => throw new NotImplementedException();
        public DateTime GetLastCommitDate(string branchName) => throw new NotImplementedException();
    }
}