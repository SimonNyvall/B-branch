using Git.Base;
using Shared.TableData;

namespace Git.Options;

public class WorkingBranchOption(IGitBase gitBase) : IOption
{
    public List<GitBranch> Execute(List<GitBranch> branches)
    {
        string workingBranchName = gitBase.GetWorkingBranch();

        return UpdateWorkingBranches(branches, workingBranchName);
    }

    private static List<GitBranch> UpdateWorkingBranches(List<GitBranch> branches, string workingBranchName)
    {
        int index = branches.FindIndex(branch => branch.Branch.Name.Equals(workingBranchName));

        if (index == -1) return branches; 

        Branch workingBranch = new(name: branches[index].Branch.Name, isWorkingBranch: true);
        branches[index].SetBranch(workingBranch);

        return branches;
    }
}