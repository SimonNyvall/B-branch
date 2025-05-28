using Bbranch.GitService.Base;
using Bbranch.Shared.TableData;

namespace Bbranch.GitService.OptionStrategies.Common;

public sealed class WorkingBranchOption(IGitRepository gitBase) : IOption
{
    public HashSet<GitBranch> Execute(HashSet<GitBranch> branches)
    {
        string workingBranchName = gitBase.GetWorkingBranch();

        return UpdateWorkingBranches(branches, workingBranchName);
    }

    private static HashSet<GitBranch> UpdateWorkingBranches(HashSet<GitBranch> branches, string workingBranchName)
    {
        var branchToUpdate = branches.FirstOrDefault(branch => branch.Branch.Name.Equals(workingBranchName));

        if (branchToUpdate == null) return branches;

        Branch workingBranch = new(name: branchToUpdate.Branch.Name, isWorkingBranch: true);
        branchToUpdate.SetBranch(workingBranch);

        return branches;
    }
}