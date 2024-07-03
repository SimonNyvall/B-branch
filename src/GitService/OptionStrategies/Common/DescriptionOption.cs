using Shared.TableData;
using Git.Base;

namespace Git.Options;

public class DescriptionOption : IOption
{
    public List<GitBranch> Execute(List<GitBranch> branches)
    {
        IGitBase gitBase = GitBase.GetInstance();

        return gitBase.GetBranchDescription(branches);
    }
}