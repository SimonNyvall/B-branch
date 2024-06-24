using Shared.TableData;

namespace Git.Options;

public class NoContainsOption(string pattern) : IOption
{
    public List<GitBranch> Execute(List<GitBranch> branches)
    {
        return branches.Where(branch => !branch.Branch.Name.Contains(pattern)).ToList(); 
    }
}