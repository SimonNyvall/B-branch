using Bbranch.Shared.TableData;
using System.Text.RegularExpressions;

namespace Bbranch.GitService.OptionStrategies.Common.ContainsStrategies;

public sealed class ContainsOption(string pattern) : IOption
{
    public HashSet<GitBranch> Execute(HashSet<GitBranch> branches)
    {
        string[] patterns = ContainsSplit.SplitArgument(pattern);

        return
        [
            .. branches.Where(branch => patterns.Any(p =>
            {
                try
                {
                    return Regex.IsMatch(branch.Branch.Name, p, RegexOptions.IgnoreCase);
                }
                catch
                {
                    return branch.Branch.Name.Contains(p, StringComparison.OrdinalIgnoreCase);
                }
            }))
        ];
    }
}
