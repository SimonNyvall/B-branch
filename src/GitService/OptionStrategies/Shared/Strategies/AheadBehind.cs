using Shared.TableData;
using Git.Base;

namespace Git.Options;

public class TrackAheadBehindOption(string remoteBranchName) : IOption
{
    public List<GitBranch> Execute(List<GitBranch> branches)
    {
        IGitBase gitBase = GitBase.GetInstance();

        for (int i = 0; i < branches.Count; i++)
        {
            GitBranch branch = branches[i];

            string arguments = $"rev-list --left-right --count {branch.Branch.Name}...origin/{remoteBranchName}";

            AheadBehind aheadBehind = gitBase.GetAheadBehind(arguments);

            branches[i].SetAheadBehind(aheadBehind);            
        }

        return branches;
    }
}

public class DefaultAheadBehindOption : IOption
{

    public List<GitBranch> Execute(List<GitBranch> branches)
    {
        IGitBase gitBase = GitBase.GetInstance();

        for (int i = 0; i < branches.Count; i++)
        {
            GitBranch branch = branches[i];

            string arguments = $"rev-list --left-right --count {branch.Branch.Name}...origin/{branch.Branch.Name}";

            AheadBehind aheadBehind = gitBase.GetAheadBehind(arguments);

            branches[i].SetAheadBehind(aheadBehind);
        }

        return branches;
    }
}