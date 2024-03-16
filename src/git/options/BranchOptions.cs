namespace Bbranch.Git.Options.BranchOptions;

using Bbranch.Git.Base.GitBase;
using Bbranch.TableData;

public class BranchOptions : GitBase
{
    public static void GetBranches(bool? allFlag, bool? remoteFlag, ref List<GitBranch> branches)
    {
        if (allFlag is not null)
        {
            branches = GetNamesAndLastWirte(true, false);
        }
        else if (remoteFlag is not null)
        {
            branches = GetNamesAndLastWirte(false, true);
        }
        else
        {
            branches = GetNamesAndLastWirte(false, false);
        }
    }
}
