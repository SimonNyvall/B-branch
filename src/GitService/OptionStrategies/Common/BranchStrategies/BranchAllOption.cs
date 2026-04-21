using Bbranch.GitService.Base;
using Bbranch.Shared.TableData;

namespace Bbranch.GitService.OptionStrategies.Common.BranchStrategies;

public sealed class BranchAllOptions(IGitRepository gitBase) : IOption
{
    public HashSet<GitBranch> Execute(HashSet<GitBranch> branches)
    {
        HashSet<GitBranch> localBranches = gitBase.GetLocalBranchNames();
        HashSet<GitBranch> remoteBranches = gitBase.GetRemoteBranchNames();

        branches.UnionWith(localBranches);
        branches.UnionWith(remoteBranches);

        return branches;
    }
}
