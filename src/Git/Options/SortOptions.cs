using Git.Base;
using TableData;

namespace Git.Options;

public class SortOptions : GitBase
{
    public static void GetBranches(
        List<BranchTableRow> branches,
        Dictionary<string, string> options,
        ref List<BranchTableRow> branchTable
    )
    {
        if (!(options.ContainsKey("sort") || options.ContainsKey("s")))
        {
            return;
        }

        List<BranchTableRow> sortedList = [];

        if (options.ContainsKey("sort"))
        {
            sortedList = GetSortedBranches(branches, ref branchTable, options, "sort");
        }
        else if (options.ContainsKey("s"))
        {
            sortedList = GetSortedBranches(branches, ref branchTable, options, "s");
        }

        branchTable = sortedList;
    }

    private static List<BranchTableRow> GetSortedBranches(
        List<BranchTableRow> branches,
        ref List<BranchTableRow> branchTable,
        Dictionary<string, string> options,
        string optionKey
    )
    {
        return (options[optionKey].ToLower()) switch
        {
            "name" => branches.OrderBy(branch => branch.BranchName).ToList(),
            "ahead" => branches.OrderByDescending(branch => branch.Ahead).ToList(),
            "behind" => branches.OrderByDescending(branch => branch.Behind).ToList(),
            "lastcommit" => branches.OrderByDescending(branch => branch.LastCommit).ToList(),
            _ => branches.OrderByDescending(branch => branch.BranchName).ToList(),
        };
    }
}
