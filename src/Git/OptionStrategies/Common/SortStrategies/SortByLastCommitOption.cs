using TableData;

namespace Git.Options;

// This is the default sort option
public class SortByLastEditedOptions : IOption
{
    public List<GitBranch> Execute(List<GitBranch> branches)
    {
        return [.. branches.OrderByDescending(branch => branch.LastCommit)];
    }
}