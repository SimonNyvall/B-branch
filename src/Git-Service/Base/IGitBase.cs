using Shared.TableData;

namespace Git.Base;

public interface IGitBase
{
    string GetWorkingBranch();

    List<GitBranch> GetLocalBranchNames();

    List<GitBranch> GetRemoteBranchNames();

    List<GitBranch> GetBranchDescription(List<GitBranch> branches);

    AheadBehind GetAheadBehind(string argument);

    DateTime GetLastCommitDate(string branchName);

    bool DoesBranchExist(string branchName);
}
