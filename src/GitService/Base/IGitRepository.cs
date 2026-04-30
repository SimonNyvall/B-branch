using Bbranch.Shared.TableData;

namespace Bbranch.GitService.Base;

public interface IGitRepository
{
    Task<string> GetWorkingBranch();

    Task<HashSet<GitBranch>> GetLocalBranchNames();

    Task<HashSet<GitBranch>> GetRemoteBranchNames();

    Task<HashSet<GitBranch>> GetBranchDescription(HashSet<GitBranch> branches);

    Task<AheadBehind> GetLocalAheadBehind(string localBranchName);

    Task<AheadBehind> GetRemoteAheadBehind(string localBranchName, string remoteBranchName);

    Task<DateTime> GetLastCommitDate(string branchName);

    HashSet<GitBranch> StichWorkTreeBranches(HashSet<GitBranch> branches);
}
