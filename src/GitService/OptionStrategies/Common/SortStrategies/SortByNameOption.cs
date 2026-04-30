using Bbranch.Shared.TableData;

namespace Bbranch.GitService.OptionStrategies.Common.SortStrategies;

public sealed class SortByNameOptions : IOption
{
    public Task<HashSet<GitBranch>> Execute(HashSet<GitBranch> branches)
    {
        HashSet<GitBranch> sortedBranches = [.. branches.OrderBy(branch => branch.Branch.Name)];
        return Task.FromResult(sortedBranches);
    }
}
