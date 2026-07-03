using Bbranch.Shared.TableData;

namespace Bbranch.GitService.Base;

public interface IGitRepository
{
    Task<string> GetWorkingBranch();

    Task<List<GitBranch>> GetLocalBranchNames();

    Task<List<GitBranch>> GetRemoteBranchNames();

    Task<List<GitBranch>> GetBranchDescription(List<GitBranch> branches);

    Task<AheadBehind> GetLocalAheadBehind(string localBranchName);

    Task<AheadBehind> GetRemoteAheadBehind(string localBranchName, string remoteBranchName);

    Task<GitBranch> GetLastCommitDate(GitBranch branchName);

    List<GitBranch> StichWorkTreeBranches(List<GitBranch> branches);
}
