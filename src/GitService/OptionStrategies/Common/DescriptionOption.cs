using Shared.TableData;
using Git.Base;

namespace Git.Options;

public class DescriptionOption : IOption
{
    public List<GitBranch> Execute(List<GitBranch> branches)
    {
        IGitRepository gitBase = GitRepository.GetInstance();

        return gitBase.GetBranchDescription(branches);
    }
}