using Bbranch.GitService.Base;
using Bbranch.Shared.TableData;

namespace Bbranch.GitService.OptionStrategies.Common;

public class WorkingBranchOption(IGitRepository gitBase) : IOption
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