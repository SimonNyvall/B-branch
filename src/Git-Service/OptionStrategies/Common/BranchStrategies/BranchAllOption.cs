using Shared.TableData;
using Git.Base;

namespace Git.Options;

public class BranchAllOptions : IOption
{
    public List<GitBranch> Execute(List<GitBranch> branches)
    {
        // TODO: sort the branches on the last edited, maybe can setup a stratigy for that later
        var gitBase = GitBase.GetInstance();

        List<GitBranch> localBranches = gitBase.GetLocalBranchNames();
        List<GitBranch> remoteBranches = gitBase.GetRemoteBranchNames();

        branches.AddRange(localBranches);
        branches.AddRange(remoteBranches);

        return branches;
    }
}