using Bbranch.Shared.TableData;

namespace Bbranch.GitService.OptionStrategies.Common.SortStrategies;

public sealed class SortByAheadOptions : IOption
{
    public Task<HashSet<GitBranch>> Execute(HashSet<GitBranch> branches)
    {
        HashSet<GitBranch> sortedBranches =
        [
            .. branches.OrderByDescending(branch => branch.AheadBehind.Ahead),
        ];

        return Task.FromResult(sortedBranches);
    }
}
