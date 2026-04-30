using Bbranch.Shared.TableData;

namespace Bbranch.GitService.OptionStrategies.Common.SortStrategies;

public sealed class SortByDetachedHeadOption : IOption
{
    public Task<HashSet<GitBranch>> Execute(HashSet<GitBranch> branches)
    {
        HashSet<GitBranch> sortedBranches =
        [
            .. branches.OrderByDescending(b => b.DetachedHead.commitHash),
        ];
        return Task.FromResult(sortedBranches);
    }
}
