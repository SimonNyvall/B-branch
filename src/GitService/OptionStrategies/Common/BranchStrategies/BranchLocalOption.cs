using Bbranch.Shared.TableData;
using Bbranch.GitService.Base;

namespace Bbranch.GitService.OptionStrategies.Common.BranchStrategies;

public sealed class BranchLocalOptions(IGitRepository gitBase) : IOption
{
    public HashSet<GitBranch> Execute(HashSet<GitBranch> branches)
    {
        return gitBase.GetLocalBranchNames();
    }
}