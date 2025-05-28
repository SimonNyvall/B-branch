using Bbranch.Shared.TableData;

namespace Bbranch.GitService.OptionStrategies.Common;

public class TopOption(int takeYield) : IOption
{
    public HashSet<GitBranch> Execute(HashSet<GitBranch> branches)
    {
        return [.. branches.Take(takeYield)];
    }
}