namespace Bbranch.Git.Options.SortOptions;

using Bbranch.Git.Base.GitBase;
using Bbranch.TableData;

public class SortOptions : GitBase
{
    public static void GetBranches(List<BranchTableRow> branches, string? sortFlag, ref List<BranchTableRow> branchTable)
    {
        if (sortFlag is not null)
        {
            branchTable = sortFlag.ToLower() switch
            {
                "name" => branches.OrderBy(branch => branch.BranchName).ToList(),
                "date" => branches.OrderByDescending(branch => branch.LastCommit).ToList(),
                "ahead" => branches.OrderByDescending(branch => branch.Ahead).ToList(),
                "behind" => branches.OrderByDescending(branch => branch.Behind).ToList(),
                _ => branches
            };
        }
    }
}
