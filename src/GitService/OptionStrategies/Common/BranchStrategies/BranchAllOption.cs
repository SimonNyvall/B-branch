using Bbranch.GitService.Base;
using Bbranch.Shared.TableData;

namespace Bbranch.GitService.OptionStrategies.Common.BranchStrategies;

public sealed class BranchAllOptions(IGitRepository gitRepository) : IOption
{
    public async Task<List<GitBranch>> Execute(List<GitBranch> branches)
    {
        var localBranchesTask = gitRepository.GetLocalBranchNames();
        var remoteBranchesTask = gitRepository.GetRemoteBranchNames();

        await Task.WhenAll(localBranchesTask, remoteBranchesTask);

        var localBranches = localBranchesTask.Result;
        var remoteBranches = remoteBranchesTask.Result;

        branches.AddRange(localBranches);
        branches.AddRange(remoteBranches);

        return branches;
    }
}
