using Bbranch.GitService.Base;
using Bbranch.Shared.TableData;

namespace Bbranch.GitService.OptionStrategies.Shared.Strategies;

public sealed class DefaultAheadBehindOption(IGitRepository gitRepository) : IOption
{
    public async Task<List<GitBranch>> Execute(List<GitBranch> branches)
    {
        var result = new List<GitBranch>();
        var aheadBehindTasks = new List<Task<GitBranch>>();

        foreach (var branch in branches)
        {
            var aheadBehindTask = gitRepository.GetLocalAheadBehind(branch);
            aheadBehindTasks.Add(aheadBehindTask);
        }

        await Task.WhenAll(aheadBehindTasks);

        foreach (var executedBranchTask in aheadBehindTasks)
        {
            var executedBranch = executedBranchTask.Result;
            var targetBranch = branches.FirstOrDefault(x => x.Id == executedBranch.Id);

            if (targetBranch == null)
                continue;

            result.Add(
                targetBranch.SetAheadBehind(
                    new AheadBehind(
                        executedBranch.AheadBehind.Ahead,
                        executedBranch.AheadBehind.Behind
                    )
                )
            );
        }

        return result;
    }
}
