using Bbranch.GitService.Base;
using Bbranch.Shared.TableData;

namespace Bbranch.GitService.OptionStrategies.Shared.Strategies;

public sealed class TrackAheadBehindOption(IGitRepository gitRepository, string remoteBranchName)
    : IOption
{
    public async Task<List<GitBranch>> Execute(List<GitBranch> branches)
    {
        var result = new List<GitBranch>();
        var aheadBehindTasks = new List<Task<GitBranch>>();

        foreach (var branch in branches)
        {
            var aheadBehindTask = gitRepository.GetRemoteAheadBehind(branch, remoteBranchName);
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
