namespace Bbranch.Git.Options.ContainsOptions;

using Bbranch.Git.Base.GitBase;
using Bbranch.TableData;

public class ContainsOptions : GitBase
{
    public static void GetBranches(string? containFlag, string? noContainFlag, ref List<GitBranch> branches)
    {
        if (containFlag is not null)
        {
            branches = branches.Where(branch => branch.Name.Contains(containFlag)).ToList();
        }

        if (noContainFlag is not null)
        {
            branches = branches.Where(branch => !branch.Name.Contains(noContainFlag)).ToList();
        }
    }
}
