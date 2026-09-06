using Bbranch.GitService.GitRepositories;
using Bbranch.Shared.TableData;

namespace Bbranch.GitService.OptionStrategies.Common;

public sealed class WorkingBranchOption(GitRepository _gitRepository) : IOption
{
    public async Task<List<GitBranch>> Execute(List<GitBranch> branches)
    {
        string workingBranchName = await _gitRepository.InternalRepo.GetWorkingBranch();

        if (string.IsNullOrEmpty(workingBranchName))
        {
            return branches;
        }

        var branchToUpdate = branches.FirstOrDefault(branch =>
            branch.Branch.Name.Equals(workingBranchName.Trim(), StringComparison.Ordinal)
        );

        if (branchToUpdate is null)
            return branches;

        var workingBranch = branchToUpdate.Branch with { IsWorkingBranch = true };
        branchToUpdate.SetBranch(workingBranch);

        return branches;
    }
}
