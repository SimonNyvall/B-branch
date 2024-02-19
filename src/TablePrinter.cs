namespace Bbranch.TablePrinter;

using Bbranch.TableData;

public class Data
{
    public static void PrintBranchTable(List<BranchTableRow> branchTable)
    {
        int minimumBranchNameWidth = 14;
        var longestBranchName = Math.Max(minimumBranchNameWidth, branchTable.Max(x => x.BranchName.Length));

        int maxLastCommitWidth = branchTable.Max(x => $"{x.LastCommit.Item1} {x.LastCommit.Item2}".Length) - 6;

        string branchHeader = " Branch Name  ".PadRight(longestBranchName);
        string underline = new string('-', longestBranchName + 1);

        string[] headers = { " Ahead 󰜘 ", " Behind 󰜘 ", branchHeader, " Last commit  " };

        Console.WriteLine();

        foreach (var header in headers)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"{header}");
            Console.ResetColor();
            Console.Write(" | ");
        }

        Console.WriteLine($"\n--------- | ---------- | {underline} | {new string('-', maxLastCommitWidth + 10)}");

        foreach (var branch in branchTable)
        {
            var aHead = branch.Ahead.ToString().PadRight(8);
            var behind = branch.Behind.ToString().PadRight(9);
            var branchName = branch.BranchName.PadRight(longestBranchName);
            var lastCommitText = $"{branch.LastCommit.Item1.PadRight(maxLastCommitWidth)} {branch.LastCommit.Item2}";

            Console.Write($" {aHead} |  {behind} |  ");

            if (branch.IsWorkingBranch)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write($"{branchName}");
                Console.ResetColor();
            }
            else
            {
                Console.Write($"{branchName}");
            }

            Console.WriteLine($" |  {lastCommitText}     {branch.description}");
        }
    }
}
