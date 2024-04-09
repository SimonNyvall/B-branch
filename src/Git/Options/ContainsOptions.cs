using Git.Base;
using TableData;

namespace Git.Options
{
    public class ContainsOptions : GitBase
    {
        public static void GetBranches(
            Dictionary<string, string> options,
            ref List<GitBranch> branches
        )
        {
            if (options.ContainsKey("contains") || options.ContainsKey("c"))
            {
                string? flag = GetOption(options, "contains", "c");

                if (string.IsNullOrEmpty(flag))
                {
                    return;
                }

                branches = branches.Where(branch => branch.Name.Contains(flag)).ToList();
            }
            else if (options.ContainsKey("noContains") || options.ContainsKey("n"))
            {
                string? flag = GetOption(options, "noContains", "n");

                if (string.IsNullOrEmpty(flag))
                {
                    return;
                }

                branches = branches.Where(branch => !branch.Name.Contains(flag)).ToList();
            }
        }
    }
}
