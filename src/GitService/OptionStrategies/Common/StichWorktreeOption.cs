using Bbranch.GitService.Base;
using Bbranch.Shared.TableData;

namespace Bbranch.GitService.OptionStrategies.Common;

public sealed class StichWorktreeOption(IGitRepository gitRepository) : IOption
{
    public Task<HashSet<GitBranch>> Execute(HashSet<GitBranch> branches)
    {
        return Task.FromResult(gitRepository.StichWorkTreeBranches(branches));
    }
}
