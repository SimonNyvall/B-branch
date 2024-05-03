using TableData;

namespace Git.Options;

public class SortByNameOptions : IOption
{
    public List<GitBranch> Execute(List<GitBranch> branches)
    {
        return branches.OrderBy(branch => branch.Branch.Name).ToList();
    }
}