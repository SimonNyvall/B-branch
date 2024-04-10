using Git.Base;
using TableData;

namespace Git.Options
{
    public class BranchOptions : GitBase
    {
        public static void GetBranches(
            Dictionary<string, string> options,
            ref List<GitBranch> branches
        )
        {
            branches =
                options.ContainsKey("all") || options.ContainsKey("a")
                    ? GetNamesAndLastWirte(true, false)
                    : options.ContainsKey("remote") || options.ContainsKey("r")
                        ? GetNamesAndLastWirte(false, true)
                        : GetNamesAndLastWirte(false, false);
        }
    }
}
