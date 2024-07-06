using Shared.TableData;
using Git.Base;

namespace Git.Options;

public class BranchRemoteOptions(IGitBase gitBase) : IOption
{
    public List<GitBranch> Execute(List<GitBranch> branches)
    {
        return gitBase.GetRemoteBranchNames();
    }
}

