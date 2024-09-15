using Bbranch.Shared.TableData;

namespace Bbranch.GitService.OptionStrategies.Common.SortStrategies;

public class SortByBehindOptions : IOption
{
    public List<GitBranch> Execute(List<GitBranch> branches)
    {
        return [.. branches.OrderByDescending(branch => branch.AheadBehind.Behind)]; 
    }
}