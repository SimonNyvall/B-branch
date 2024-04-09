using Git.Base;
using TableData;

namespace Git.Options
{
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

            IOrderedEnumerable<BranchTableRow> sortedList = string.Compare(
                options["sort"],
                "name",
                StringComparison.OrdinalIgnoreCase
            ) switch
            {
                0 => branches.OrderBy(branch => branch.BranchName),
                1 => branches.OrderByDescending(branch => branch.Ahead),
                2 => branches.OrderByDescending(branch => branch.Behind),
                _ => branches.OrderByDescending(branch => branch.LastCommit),
            };

            branchTable = sortedList.Cast<BranchTableRow>().ToList();
        }
    }
}
