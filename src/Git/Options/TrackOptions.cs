using Git.Base;
using TableData;

namespace Git.Options;

public class TrackOptions : GitBase
{
    public static async Task<List<BranchTableRow>> GetBranches(
        Dictionary<string, string> options,
        List<GitBranch> branches,
        string workingBranch
    )
    {
        if (options.ContainsKey("track") || options.ContainsKey("t"))
        {
            string? flag = GetOption(options, "track", "t");

            if (flag is null)
            {
                return await Project.MapGitBranches(branches, workingBranch);
            }

            return await Project.MapGitBranches(branches, workingBranch, flag);
        }

        return await Project.MapGitBranches(branches, workingBranch);
    }
}
