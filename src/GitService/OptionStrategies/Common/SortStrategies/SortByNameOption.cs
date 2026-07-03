using Bbranch.Shared.TableData;

namespace Bbranch.GitService.OptionStrategies.Common.SortStrategies;

public sealed class SortByNameOptions : IOption
{
    public Task<List<GitBranch>> Execute(List<GitBranch> branches)
    {
        List<GitBranch> sortedBranches = [.. branches.OrderBy(branch => branch.Branch.Name)];
        return Task.FromResult(sortedBranches);
    }
}
