using System.Text.RegularExpressions;
using Bbranch.Shared.TableData;

namespace Bbranch.GitService.OptionStrategies.Common.ContainsStrategies;

public sealed class NoContainsOption(string pattern) : IOption
{
    public Task<HashSet<GitBranch>> Execute(HashSet<GitBranch> branches)
    {
        string[] patterns = ContainsSplit.SplitArgument(pattern);

        HashSet<GitBranch> filteredBranches =
        [
            .. branches.Where(branch =>
                patterns.All(p =>
                {
                    try
                    {
                        return !Regex.IsMatch(branch.Branch.Name, p, RegexOptions.IgnoreCase);
                    }
                    catch
                    {
                        return !branch.Branch.Name.Contains(p, StringComparison.OrdinalIgnoreCase);
                    }
                })
            ),
        ];

        return Task.FromResult(filteredBranches);
    }
}
