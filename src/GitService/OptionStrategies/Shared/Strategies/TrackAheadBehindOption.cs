using Shared.TableData;
using Git.Base;

namespace Git.Options;

public class TrackAheadBehindOption(IGitBase gitBase, string remoteBranchName) : IOption
{
    public List<GitBranch> Execute(List<GitBranch> branches)
    {
        foreach (GitBranch branch in branches)
        {
            string arguments = $"rev-list --left-right --count {branch.Branch.Name}...{remoteBranchName}";

            AheadBehind aheadBehind = gitBase.GetAheadBehind(arguments);

            branch.SetAheadBehind(aheadBehind);            
        }

        return branches;
    }
}