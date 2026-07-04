using Bbranch.Shared.TableData;

namespace Bbranch.GitService.OptionStrategies.Common;

public sealed class TopOption(int takeYield) : IOption
{
    public Task<List<GitBranch>> Execute(List<GitBranch> branches)
    {
        List<GitBranch> topBranches = [.. branches.Take(takeYield)];
        return Task.FromResult(topBranches);
    }
}
