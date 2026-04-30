using Bbranch.Shared.TableData;

namespace Bbranch.GitService.OptionStrategies.Common.SortStrategies;

public sealed class SortBySymbolicOption : IOption
{
    public Task<HashSet<GitBranch>> Execute(HashSet<GitBranch> branches)
    {
        HashSet<GitBranch> sortedBranches =
        [
            .. branches.OrderByDescending(branch => branch.IsSymbolic),
        ];
        return Task.FromResult(sortedBranches);
    }
}
