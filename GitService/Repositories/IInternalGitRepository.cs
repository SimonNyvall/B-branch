using Bbranch.Shared.TableData;

namespace Bbranch.GitService.GitRepositories;

public interface IInternalGitRepository
{
    Task<string> GetWorkingBranch();

    IAsyncEnumerable<List<GitBranch>> GetLocalBranchNames(int batchSize);

    IAsyncEnumerable<List<GitBranch>> GetRemoteBranchNames(int batchSize);

    Task<List<GitBranch>> GetBranchDescription(List<GitBranch> branches);

    Task<GitBranch> GetLocalAheadBehind(GitBranch localBranchName);

    Task<GitBranch> GetRemoteAheadBehind(GitBranch gitBranch, string remoteBranchName);

    Task<GitBranch> GetLastCommitDate(GitBranch branchName);

    List<GitBranch> StichWorkTreeBranches(List<GitBranch> branches);

    bool IsBranchMerged(string branchName);
}
