using Git.Base;
using Shared.TableData;

namespace Git.Options;

public class DefaultAheadBehindOption(IGitBase gitBase) : IOption
{
    public List<GitBranch> Execute(List<GitBranch> branches)
    {
        foreach (GitBranch branch in branches)
        {
            string arguments = $"rev-list --left-right --count {branch.Branch.Name}...origin/{branch.Branch.Name}";

            AheadBehind aheadBehind = gitBase.GetAheadBehind(arguments);

            branch.SetAheadBehind(aheadBehind);
        }

        return branches;
    }
}
