using Bbranch.Shared.TableData;

namespace Bbranch.GitService.OptionStrategies.Common.SortStrategies;

public sealed class SortByNameOptions : IOption
{
    public HashSet<GitBranch> Execute(HashSet<GitBranch> branches)
    {
        return [.. branches.OrderBy(branch => branch.Branch.Name)];
    }
}