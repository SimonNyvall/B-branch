namespace Bbranch.Output.Data;

using Bbranch.Table;

public class Data
{
    public static void PrintBranchTable(List<Table> branchTable)
    {
        int minimumBranchNameWidth = 11;
        var longestBranchName = Math.Max(minimumBranchNameWidth, branchTable.Max(x => x.BranchName.Length));

        string branchHeader = "Branch Name".PadRight(longestBranchName);
        string underline = new string('-', longestBranchName);

        Console.WriteLine($"Ahead  |  Behind  | {branchHeader} | Last commit");
        Console.WriteLine($"------ | -------- | {underline} | -----------");

        foreach (var branch in branchTable)
        {
            var aHead = branch.Ahead.ToString().PadRight(6);
            var behind = branch.Behind.ToString().PadRight(8);
            var branchName = branch.BranchName.PadRight(longestBranchName);

            Console.WriteLine($"{aHead} | {behind} | {branchName} | {branch.LastCommit.Item1} {branch.LastCommit.Item2}");
        }
    }
}
