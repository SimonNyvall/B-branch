using Bbranch.Shared.TableData;

namespace Bbranch.GitService.OptionStrategies.Common.SortStrategies;

/// <summary>
/// Sorts branches by their last commit date in descending order.
/// /// This means the most recently updated branches will appear first.
/// </summary>
public sealed class SortByLastCommitOptions : IOption
{
    public HashSet<GitBranch> Execute(HashSet<GitBranch> branches)
    {
        return [.. branches.OrderByDescending(branch => branch.LastCommit)];
    }
}