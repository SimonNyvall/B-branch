using Bbranch.GitService.Base;
using Bbranch.Shared.TableData;

namespace Bbranch.GitService.OptionStrategies.Shared.Setters;

public sealed class SetLastCommitOptions(IGitRepository gitBase) : IOption
{
    public HashSet<GitBranch> Execute(HashSet<GitBranch> branches)
    {
        foreach (GitBranch branch in branches)
        {
            var branchName = branch.Branch.Name;

            if (branch.DetachedHead.commitHash != null)
            {
                branchName = branch.DetachedHead.commitHash;
            }

            DateTime lastCommit = gitBase.GetLastCommitDate(branchName);

            branch.SetLastCommit(lastCommit);
        }

        return branches;
    }
}
