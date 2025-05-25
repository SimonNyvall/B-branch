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
        var branches = new List<GitBranch>
        {
            GitBranch.Default().SetBranch(new Branch("main", isWorkingBranch: false)),
            GitBranch.Default().SetBranch(new Branch("feature", isWorkingBranch: false))
        };

        var workingBranchOption = new WorkingBranchOption(_gitBase);

        List<GitBranch> result = workingBranchOption.Execute(branches);

        Assert.True(result[0].Branch.IsWorkingBranch);
        Assert.False(result[1].Branch.IsWorkingBranch);
    }

    [Fact]
    public void Given_WorkingBranchOption_When_ExecuteRun_Then_Return_EmptyList()
    {
        List<GitBranch> branches = [];

        var workingBranchOption = new WorkingBranchOption(_gitBase);

        List<GitBranch> result = workingBranchOption.Execute(branches);

        Assert.Empty(result);
    }
    
    private sealed class GitRepositoryMock : IGitRepository
    {
        public string GetWorkingBranch()
        {
            return "main";
        }

        public List<GitBranch> GetLocalBranchNames() => throw new NotImplementedException();
        public List<GitBranch> GetRemoteBranchNames() => throw new NotImplementedException();
        public List<GitBranch> GetBranchDescription(List<GitBranch> branches) => throw new NotImplementedException();
        public AheadBehind GetLocalAheadBehind(string localBranchName) => throw new NotImplementedException();
        public AheadBehind GetRemoteAheadBehind(string localBranchName, string remoteBranchName) => throw new NotImplementedException();
        public DateTime GetLastCommitDate(string branchName) => throw new NotImplementedException();
        public bool DoesBranchExist(string branchName) => throw new NotImplementedException();
    }
}