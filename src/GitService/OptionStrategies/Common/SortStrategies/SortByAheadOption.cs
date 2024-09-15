using Bbranch.Shared.TableData;

namespace Bbranch.GitService.OptionStrategies.Common.SortStrategies;

public class SortByAheadOptions : IOption
{
    public List<GitBranch> Execute(List<GitBranch> branches)
    {
        return [.. branches.OrderByDescending(branch => branch.AheadBehind.Ahead)];
    }
}