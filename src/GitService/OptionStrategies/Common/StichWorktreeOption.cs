using Bbranch.GitService.Base;
using Bbranch.Shared.TableData;

namespace Bbranch.GitService.OptionStrategies.Common;

public sealed class StichWorktreeOption(IGitRepository gitRepository) : IOption
{
    public Task<List<GitBranch>> Execute(List<GitBranch> branches)
    {
        return Task.FromResult(gitRepository.StichWorkTreeBranches(branches));
    }
}
