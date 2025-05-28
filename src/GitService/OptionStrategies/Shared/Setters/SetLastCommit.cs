using Bbranch.Shared.TableData;
using Bbranch.GitService.Base;

namespace Bbranch.GitService.OptionStrategies.Shared.Setters;

public sealed class SetLastCommitOptions(IGitRepository gitBase) : IOption
{
    public HashSet<GitBranch> Execute(HashSet<GitBranch> branches)
    {
        foreach (GitBranch branch in branches)
        {
            DateTime lastCommit = gitBase.GetLastCommitDate(branch.Branch.Name);

            branch.SetLastCommit(lastCommit);
        }

        return branches;
    }
}
