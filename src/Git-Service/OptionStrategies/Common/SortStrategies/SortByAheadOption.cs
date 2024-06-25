using Shared.TableData;

namespace Git.Options;

public class SortByAheadOptions : IOption
{
    public List<GitBranch> Execute(List<GitBranch> branches)
    {
        return branches.OrderByDescending(branch => branch.AheadBehind.Ahead).ToList();
    }
}