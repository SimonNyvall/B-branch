using Bbranch.Shared.TableData;

namespace Bbranch.GitService.Base;

public interface IGitRepository
{
    Task<string> GetWorkingBranch();

    Task<List<GitBranch>> GetLocalBranchNames();

    Task<List<GitBranch>> GetRemoteBranchNames();

    Task<List<GitBranch>> GetBranchDescription(List<GitBranch> branches);

    Task<GitBranch> GetLocalAheadBehind(GitBranch localBranchName);

    Task<GitBranch> GetRemoteAheadBehind(GitBranch gitBranch, string remoteBranchName);

    Task<GitBranch> GetLastCommitDate(GitBranch branchName);

    List<GitBranch> StichWorkTreeBranches(List<GitBranch> branches);
}
