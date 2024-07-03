using Shared.TableData;

namespace Git.Options;

public class SortByBehindOptions : IOption
{
    public List<GitBranch> Execute(List<GitBranch> branches)
    {
        return branches.OrderByDescending(branch => branch.AheadBehind.Behind).ToList(); 
    }
}