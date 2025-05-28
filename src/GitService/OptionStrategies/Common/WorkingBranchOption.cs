using Bbranch.GitService.Base;
using Bbranch.Shared.TableData;

namespace Bbranch.GitService.OptionStrategies.Common;

public sealed class WorkingBranchOption(IGitRepository gitBase) : IOption
{
    public HashSet<GitBranch> Execute(HashSet<GitBranch> branches)
    {
        string workingBranchName = gitBase.GetWorkingBranch();
        
        if (string.IsNullOrEmpty(workingBranchName))
        {
            return branches;
        }

        var branchToUpdate = branches.FirstOrDefault(branch => 
            branch.Branch.Name.Equals(workingBranchName, StringComparison.Ordinal));

        if (branchToUpdate is null) return branches;

        var workingBranch = branchToUpdate.Branch with { IsWorkingBranch = true };
        branchToUpdate.SetBranch(workingBranch);

        return branches;
    }
}