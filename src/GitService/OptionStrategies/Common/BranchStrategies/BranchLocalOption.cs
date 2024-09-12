using Shared.TableData;
using Git.Base;

namespace Git.Options;

public class BranchLocalOptions(IGitRepository gitBase) : IOption
{
    public List<GitBranch> Execute(List<GitBranch> branches)
    {
        return gitBase.GetLocalBranchNames();
    }
}