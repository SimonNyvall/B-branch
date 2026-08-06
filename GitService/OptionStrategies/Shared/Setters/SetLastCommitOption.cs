using Bbranch.GitService.Base;
using Bbranch.Shared.TableData;

namespace Bbranch.GitService.OptionStrategies.Shared.Setters;

public sealed class SetLastCommitOptions(IGitRepository gitBase) : IOption
{
    public async Task<List<GitBranch>> Execute(List<GitBranch> branches)
    {
        var packedBranches = branches.Where(x => x.IsPacked).ToList();
        var unpackedBranches = branches.Where(x => !x.IsPacked).ToList();

        var command = new LastPackedCommitDateFetchCommand(packedBranches);
        var packedBranchesResultTask = command.Execute();

        var branchesResultTasks = new List<Task<GitBranch>>();

        foreach (GitBranch branch in unpackedBranches)
        {
            var branchName = branch.Branch.Name;

            if (branch.DetachedHead.commitHash != null)
            {
                branchName = branch.DetachedHead.commitHash;
            }

            if (branch.IsSymbolic)
            {
                branchName = branch.SymLink!.Target;
            }

            branchesResultTasks.Add(gitBase.GetLastCommitDate(branch));
        }

        await Task.WhenAll(
            branchesResultTasks.Cast<Task>().Concat(new[] { packedBranchesResultTask })
        );

        return branches;
    }
}
