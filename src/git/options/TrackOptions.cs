namespace Bbranch.Git.Options.TrackOptions;

using Bbranch.Git.Base.GitBase;
using Bbranch.TableData;

public class TrackOptions : GitBase
{
    public static async Task<List<BranchTableRow>> GetBranches(string? trackFlag, GitBase gitBase, List<GitBranch> branches, string workingBranch)
    {
        if (trackFlag is null)
        {
            return await Project.MapGitBranches(gitBase, branches, workingBranch);
        }

        return await Project.MapGitBranches(gitBase, branches, workingBranch, trackFlag);
    }
}
