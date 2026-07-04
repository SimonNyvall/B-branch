using Bbranch.Shared.TableData;

namespace Bbranch.GitService.OptionStrategies.Common.SortStrategies;

public sealed class SortByBehindOptions : IOption
{
    public Task<List<GitBranch>> Execute(List<GitBranch> branches)
    {
        List<GitBranch> sortedBranches =
        [
            .. branches.OrderByDescending(branch => branch.AheadBehind.Behind),
        ];
        return Task.FromResult(sortedBranches);
    }
}
