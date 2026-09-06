using Bbranch.GitService.GitRepositories;
using Bbranch.Shared.TableData;

namespace Bbranch.GitService.OptionStrategies.Common.SortStrategies;

public sealed class DescriptionOption(GitRepository gitRepository) : IOption
{
    public async Task<List<GitBranch>> Execute(List<GitBranch> branches)
    {
        return await gitRepository.InternalRepo.GetBranchDescription(branches);
    }
}
