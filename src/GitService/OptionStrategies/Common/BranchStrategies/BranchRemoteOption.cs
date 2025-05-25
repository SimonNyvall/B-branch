using Bbranch.Shared.TableData;
using Bbranch.GitService.Base;

namespace Bbranch.GitService.OptionStrategies.Common.BranchStrategies;

public sealed class BranchRemoteOptions(IGitRepository gitBase) : IOption
{
    public List<GitBranch> Execute(List<GitBranch> branches)
    {
        return gitBase.GetRemoteBranchNames();
    }
}

