using Shared.TableData;
using Git.Base;

namespace Git.Options;

public class WorkingBranchOption : IOption
{
    public List<GitBranch> Execute(List<GitBranch> branches)
    {
        IGitBase gitBase = GitBase.GetInstance();

        string workingBranchName = gitBase.GetWorkingBranch();

        return NewMethod(branches, workingBranchName);
    }

    private static List<GitBranch> NewMethod(List<GitBranch> branches, string workingBranchName)
    {
        foreach (GitBranch branch in branches)
        {
            if (!branch.Branch.Name.Equals(workingBranchName))
            {
                continue;
            }

            Branch workingBranch = new() { Name = branch.Branch.Name, IsWorkingBranch = true };
            branch.SetBranch(workingBranch);
        }

        return branches;
    }
}