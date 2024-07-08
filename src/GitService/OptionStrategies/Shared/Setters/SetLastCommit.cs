using Shared.TableData;
using Git.Base;

namespace Git.Options;

public class SetLastCommitOptions(IGitBase gitBase) : IOption
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
