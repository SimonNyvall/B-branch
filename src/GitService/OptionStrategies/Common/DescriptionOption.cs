using Bbranch.GitService.Base;
using Bbranch.Shared.TableData;

namespace Bbranch.GitService.OptionStrategies.Common.SortStrategies;

public sealed class DescriptionOption(IGitRepository gitRepository) : IOption
{
    public async Task<List<GitBranch>> Execute(List<GitBranch> branches)
    {
        return await gitRepository.GetBranchDescription(branches);
    }
}
