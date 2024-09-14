using Bbranch.Shared.TableData;
using Bbranch.GitService.Base;

namespace Bbranch.GitService.OptionStrategies.Common.SortStrategies;

public class DescriptionOption : IOption
{
    public List<GitBranch> Execute(List<GitBranch> branches)
    {
        IGitRepository gitBase = GitRepository.GetInstance();

        return gitBase.GetBranchDescription(branches);
    }
}