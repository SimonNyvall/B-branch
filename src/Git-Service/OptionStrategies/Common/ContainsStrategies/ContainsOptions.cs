using Shared.TableData;

namespace Git.Options;

public class ContainsOption(string pattern) : IOption
{
    public List<GitBranch> Execute(List<GitBranch> branches)
    {
        return branches.Where(branch => branch.Branch.Name.Contains(pattern)).ToList();
    }
}