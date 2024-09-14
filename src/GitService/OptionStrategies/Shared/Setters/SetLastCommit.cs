using Bbranch.Shared.TableData;
using Bbranch.GitService.Base;

namespace Bbranch.GitService.OptionStrategies.Shared.Setters;

public class SetLastCommitOptions(IGitRepository gitBase) : IOption
{
    public List<GitBranch> Execute(List<GitBranch> branches)
    {
        foreach (GitBranch branch in branches)
        {
            DateTime lastCommit = gitBase.GetLastCommitDate(branch.Branch.Name);

            branch.SetLastCommit(lastCommit);
        }

        return branches;
    }
}
