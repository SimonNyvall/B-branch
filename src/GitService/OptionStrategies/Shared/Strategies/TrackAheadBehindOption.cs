using Shared.TableData;
using Git.Base;

namespace Git.Options;

public class TrackAheadBehindOption(IGitRepository gitBase, string remoteBranchName) : IOption
{
    public List<GitBranch> Execute(List<GitBranch> branches)
    {
        foreach (GitBranch branch in branches)
        {
            AheadBehind aheadBehind = gitBase.GetRemoteAheadBehind(branch.Branch.Name, remoteBranchName);

            branch.SetAheadBehind(aheadBehind);            
        }

        return branches;
    }
}