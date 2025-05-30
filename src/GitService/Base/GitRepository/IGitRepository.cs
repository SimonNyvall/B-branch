using Bbranch.Shared.TableData;

namespace Bbranch.GitService.Base;

public interface IGitRepository
{
    string GetWorkingBranch();

    HashSet<GitBranch> GetLocalBranchNames();

    HashSet<GitBranch> GetRemoteBranchNames();

    HashSet<GitBranch> GetBranchDescription(HashSet<GitBranch> branches);

    Task<AheadBehind> GetLocalAheadBehind(string localBranchName);

    Task<AheadBehind> GetRemoteAheadBehind(string localBranchName, string remoteBranchName);

    DateTime GetLastCommitDate(string branchName);
}
