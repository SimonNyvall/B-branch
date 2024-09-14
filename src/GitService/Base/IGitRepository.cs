using Shared.TableData;

namespace Git.Base;

public interface IGitRepository
{
    string GetWorkingBranch();

    List<GitBranch> GetLocalBranchNames();

    List<GitBranch> GetRemoteBranchNames();

    List<GitBranch> GetBranchDescription(List<GitBranch> branches);

    AheadBehind GetLocalAheadBehind(string localBranchName);

    AheadBehind GetRemoteAheadBehind(string localBranchName, string remoteBranchName);

    DateTime GetLastCommitDate(string branchName);

    bool DoesBranchExist(string branchName);
}
