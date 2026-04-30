using Bbranch.GitService.Base;
using Bbranch.Shared.TableData;

namespace Bbranch.GitService.OptionStrategies.Common.BranchStrategies;

public sealed class BranchRemoteOptions(IGitRepository gitBase) : IOption
{
    public async Task<HashSet<GitBranch>> Execute(HashSet<GitBranch> branches)
    {
        return await gitBase.GetRemoteBranchNames();
    }
}
