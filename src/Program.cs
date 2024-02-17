using Bbranch.Branch.Info;
using Bbranch.Branch.TablePrinter;
using Bbranch.Branch.TableData;

var branchInfo = new BranchInfo();

var gitPath = branchInfo.GitPath;

if (gitPath is null) throw new Exception("There is no .git directory in the current path");

var workingBranch = branchInfo.GetWorkingBranch(gitPath);

if (workingBranch is null) throw new Exception("There is no working branch in the current path");

var barnchTable = MapBranches(branchInfo.GetNamesAndLastWirte(gitPath), workingBranch);

Data.PrintBranchTable(barnchTable);

List<TableRow> MapBranches(Dictionary<string, DateTime> branches, string workingBranch)
{
    var branchTable = new List<TableRow>();

    foreach (var branch in branches)
    {
        var (ahead, behind) = branchInfo.GetAheadBehind(gitPath, branch.Key);

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

        if (branch.Key == workingBranch)
        {
            branchTable.Add(new TableRow(ahead, behind, branch.Key, (lastCommit, lastCommitString), true));
            continue;
        }

        branchTable.Add(new TableRow(ahead, behind, branch.Key, (lastCommit, lastCommitString), false));
    }

    return branchTable;
}
