using Bbranch.Shared.TableData;
using Bbranch.GitService.Base;

namespace Bbranch.GitService.OptionStrategies.Common.BranchStrategies;

public class BranchAllOptions(IGitRepository gitBase) : IOption
{
    public List<GitBranch> Execute(List<GitBranch> branches)
    {
        List<GitBranch> localBranches = gitBase.GetLocalBranchNames();
        List<GitBranch> remoteBranches = gitBase.GetRemoteBranchNames();

        branches.AddRange(localBranches);
        branches.AddRange(remoteBranches);

        return branches;
    }
}