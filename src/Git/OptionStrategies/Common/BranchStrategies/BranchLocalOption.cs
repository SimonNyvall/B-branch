using TableData;
using Git.Base;

namespace Git.Options;

public class BranchLocalOptions : IOption
{
    public List<GitBranch> Execute(List<GitBranch> branches)
    {
        var gitBase = GitBase.GetInstance();

        return gitBase.GetLocalBranchNames();
    }
}