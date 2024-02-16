using Bbranch.Branch.Info;
using Bbranch.Output.Data;
using Bbranch.Table;

var branchInfo = new Branch();

var gitPath = branchInfo.GitPath;

if (gitPath is null) throw new Exception("There is no .git directory in the current path");

var barnchTable = MapBranches(branchInfo.GetNamesAndLastWirte(gitPath));

Data.PrintBranchTable(barnchTable);

List<Table> MapBranches(Dictionary<string, DateTime> branches)
{
    var branchTable = new List<Table>();

    foreach (var branch in branches)
    {
        var (ahead, behind) = branchInfo.GetAheadBehind(gitPath, branch.Key);
        var lastCommit = (DateTime.Now - branch.Value).Days;

        branchTable.Add(new Table(ahead, behind, branch.Key, (lastCommit, "Days ago")));
    }

    return branchTable;
}
