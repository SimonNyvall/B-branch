using Git.Base;
using TableData;

namespace Git.Options;

public class BranchRemoteOptions : IOption
{
    public List<GitBranch> Execute(List<GitBranch> branches)
    {
        var gitBase = GitBase.GetInstance();

        return gitBase.GetRemoteBranchNames();
    }
}

