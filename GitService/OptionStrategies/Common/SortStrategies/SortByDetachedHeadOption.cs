using Bbranch.Shared.TableData;

namespace Bbranch.GitService.OptionStrategies.Common.SortStrategies;

public sealed class SortByDetachedHeadOption : IOption
{
    public Task<List<GitBranch>> Execute(List<GitBranch> branches)
    {
        List<GitBranch> sortedBranches =
        [
            .. branches.OrderByDescending(b => b.DetachedHead.commitHash),
        ];
        return Task.FromResult(sortedBranches);
    }
}
