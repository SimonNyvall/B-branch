using Shared.TableData;

namespace Git.Options;

public class NoContainsOption(string pattern) : IOption
{
    public List<GitBranch> Execute(List<GitBranch> branches)
    {
        string[] patterns = ContainsSplit.SplitArgument(pattern);

        return branches.Where(branch => patterns.All(pattern => !branch.Branch.Name.Contains(pattern))).ToList();
    }
}