using Bbranch.Shared.TableData;

namespace Bbranch.GitService.OptionStrategies.Common.SortStrategies;

/// <summary>
/// Sorts branches by their last commit date in descending order.
/// </summary>
public sealed class SortByLastCommitOptions : IOption
{
    public Task<HashSet<GitBranch>> Execute(HashSet<GitBranch> branches)
    {
        HashSet<GitBranch> sortedBranches =
        [
            .. branches.OrderByDescending(branch => branch.LastCommit),
        ];
        return Task.FromResult(sortedBranches);
    }
}
