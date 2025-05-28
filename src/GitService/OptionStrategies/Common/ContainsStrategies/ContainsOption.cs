using Bbranch.Shared.TableData;

namespace Bbranch.GitService.OptionStrategies.Common.ContainsStrategies;

public sealed class ContainsOption(string pattern) : IOption
{
    public HashSet<GitBranch> Execute(HashSet<GitBranch> branches)
    {
        string[] patterns = ContainsSplit.SplitArgument(pattern);

        return [.. branches.Where(branch => patterns.Any(pattern => branch.Branch.Name.Contains(pattern)))];
    }
}