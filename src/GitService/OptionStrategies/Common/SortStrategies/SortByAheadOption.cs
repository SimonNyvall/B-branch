using Bbranch.Shared.TableData;

namespace Bbranch.GitService.OptionStrategies.Common.SortStrategies;

public sealed class SortByAheadOptions : IOption
{
    public HashSet<GitBranch> Execute(HashSet<GitBranch> branches)
    {
        return [.. branches.OrderByDescending(branch => branch.AheadBehind.Ahead)];
    }
}