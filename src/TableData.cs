namespace Bbranch.Branch.TableData;

//public record Branch(string Branch, DateTime LastCommit);

public record BranchTableRow(int Ahead, int Behind, string BranchName, (string, string) LastCommit, bool IsWorkingBranch, string description);
/*
public class Map
{
    public static List<BranchTableRow> BranchesToTable(List<Branch> branches)
    {
        var branchTable = new List<BranchTableRow>();

        foreach (var branch in branches)
        {
            branchTable.Add(new TableRow(branch.Ahead, branch.Behind, branch.Name, branch.LastCommit, branch.IsWorkingBranch, branch.Description));
        }

        return branchTable;
    }
}*/
