using Bbranch.GitService.Base;
using Bbranch.Shared.TableData;

namespace Bbranch.GitService.OptionStrategies.Common.SortStrategies;

public sealed class DescriptionOption : IOption
{
    public HashSet<GitBranch> Execute(HashSet<GitBranch> branches)
    {
        IGitRepository gitBase = GitRepository.GetInstance();

        return gitBase.GetBranchDescription(branches);
    }
}
