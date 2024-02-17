using Bbranch.Branch.Info;
using Bbranch.Branch.TablePrinter;
using Bbranch.Branch.TableData;

var branchInfo = new BranchInfo();

var gitPath = branchInfo.GitPath;

if (gitPath is null) throw new Exception("There is no .git directory in the current path");

var barnchTable = MapBranches(branchInfo.GetNamesAndLastWirte(gitPath));

Data.PrintBranchTable(barnchTable);

List<TableRow> MapBranches(Dictionary<string, DateTime> branches)
{
    var branchTable = new List<TableRow>();

    foreach (var branch in branches)
    {
        var (ahead, behind) = branchInfo.GetAheadBehind(gitPath, branch.Key);

        string lastCommit = string.Empty;
        string lastCommitString = String.Empty;
        if ((DateTime.Now.Day - branch.Value.Day) == 0)
        {
            lastCommit = branch.Value.ToString("HH:mm");
            lastCommitString = "Today";
        }
        else
        {
            lastCommit = branch.Value.Day.ToString();
            lastCommitString = "Days ago";
        }

        branchTable.Add(new TableRow(ahead, behind, branch.Key, (lastCommit, lastCommitString)));
    }

    return branchTable;
}
