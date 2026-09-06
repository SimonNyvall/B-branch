using Bbranch.GitService.GitRepositories;
using Bbranch.Shared.TableData;

namespace Bbranch.GitService.OptionStrategies.Common;

public sealed class StichWorktreeOption(GitRepository gitRepository) : IOption
{
    public Task<List<GitBranch>> Execute(List<GitBranch> branches)
    {
        return Task.FromResult(gitRepository.InternalRepo.StichWorkTreeBranches(branches));
    }
}
