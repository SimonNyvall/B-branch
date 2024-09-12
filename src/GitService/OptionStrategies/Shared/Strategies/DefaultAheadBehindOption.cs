using Git.Base;
using Shared.TableData;

namespace Git.Options;

public class DefaultAheadBehindOption(IGitRepository gitBase) : IOption
{
    public List<GitBranch> Execute(List<GitBranch> branches)
    {
        foreach (GitBranch branch in branches)
        {
            AheadBehind aheadBehind = gitBase.GetLocalAheadBehind(branch.Branch.Name);

            branch.SetAheadBehind(aheadBehind);
        }

        return branches;
    }
}
