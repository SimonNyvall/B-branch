using Shared.TableData;
using Git.Base;

namespace Git.Options;

public class BranchLocalOptions(IGitBase gitBase) : IOption
{
    public List<GitBranch> Execute(List<GitBranch> branches)
    {
        return gitBase.GetLocalBranchNames();
    }
}