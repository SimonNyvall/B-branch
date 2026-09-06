using Bbranch.Shared.TableData;

namespace Bbranch.GitService.OptionStrategies.Common.SortStrategies;

public sealed class SortBySymbolicOption : IOption
{
    public Task<List<GitBranch>> Execute(List<GitBranch> branches)
    {
        List<GitBranch> sortedBranches =
        [
            .. branches.OrderByDescending(branch => branch.IsSymbolic),
        ];
        return Task.FromResult(sortedBranches);
    }
}
