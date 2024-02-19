using Bbranch.Branch.Info;
using Bbranch.Branch.TablePrinter;
using Bbranch.Branch.TableData;
using Bbranch.Branch.ErrorHandling;

var branchInfo = new BranchInfo();

var gitPath = branchInfo.GitPath;

Error.HandleGitDirNotFound(gitPath);

var workingBranch = branchInfo.GetWorkingBranch(gitPath!);

Error.HandleNoWorkingBranch(workingBranch);

var barnchTable = MapBranches(branchInfo.GetNamesAndLastWirte(gitPath!), workingBranch!);

Data.PrintBranchTable(barnchTable);

List<BranchTableRow> MapBranches(Dictionary<string, DateTime> branches, string workingBranch)
{
    var branchTable = new List<BranchTableRow>();

    foreach (var branch in branches)
    {
        var (ahead, behind) = branchInfo.GetAheadBehind(gitPath!, branch.Key);

        string lastCommit = string.Empty;
        string lastCommitString = String.Empty;
        int days = DateTime.Now.Day - branch.Value.Day;

        if (days == 0)
        {
            lastCommit = branch.Value.ToString("HH:mm");
            lastCommitString = "Today";
        }
        else
        {
            lastCommit = days.ToString();
            lastCommitString = days == 1 ? "Day ago" : "Days ago";
        }

        var description = branchInfo.GetBranchDescription(gitPath!, branch.Key);

        if (branch.Key == workingBranch)
        {
            branchTable.Add(new BranchTableRow(ahead, behind, branch.Key, (lastCommit, lastCommitString), true, description));
            continue;
        }

        branchTable.Add(new BranchTableRow(ahead, behind, branch.Key, (lastCommit, lastCommitString), false, description));
    }

    return branchTable;
}
