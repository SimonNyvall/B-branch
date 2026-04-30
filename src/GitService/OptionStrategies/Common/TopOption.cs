using Bbranch.Shared.TableData;

namespace Bbranch.GitService.OptionStrategies.Common;

public sealed class TopOption(int takeYield) : IOption
{
    public Task<HashSet<GitBranch>> Execute(HashSet<GitBranch> branches)
    {
        HashSet<GitBranch> topBranches = [.. branches.Take(takeYield)];
        return Task.FromResult(topBranches);
    }
}
