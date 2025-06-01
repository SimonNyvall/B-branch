using Bbranch.Shared.TableData;

namespace Bbranch.GitService.Base;

public interface IGitRepository
{
    string GitRepositoryPath { get; }
    string GetWorkingBranch();

    HashSet<GitBranch> GetLocalBranchNames();

    HashSet<GitBranch> GetRemoteBranchNames();

    HashSet<GitBranch> GetBranchDescription(HashSet<GitBranch> branches);

    DateTime GetLastCommitDate(string branchName);
}
