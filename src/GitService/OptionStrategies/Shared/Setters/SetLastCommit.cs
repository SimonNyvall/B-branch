using Shared.TableData;
using Git.Base;

namespace Git.Options;

// TODO set the last commit date is not part of the sort job, maybe we can move it to a different place
// TODO create a new set of classes for setting the default things like last commit date and working branch
public class SetLastCommitOptions : IOption
{
    public List<GitBranch> Execute(List<GitBranch> branches)
    {
        IGitBase gitBase = GitBase.GetInstance();

        foreach (GitBranch branch in branches)
        {
            DateTime lastCommit = gitBase.GetLastCommitDate(branch.Branch.Name);

            branch.SetLastCommit(lastCommit);
        }

        return branches;
    }
}
