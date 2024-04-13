using TableData;

namespace Bbranch.TablePrinter;

public class Data
{
    public static void PrintBranchTable(
        List<BranchTableRow> branchTable,
        Dictionary<string, string> options
    )
    {
        const int minimumBranchNameWidth = 14;
        int longestBranchName = Math.Max(
            minimumBranchNameWidth,
            branchTable.Max(x => x.BranchName.Length + 1)
        );
        int maxLastCommitWidth =
            branchTable.Max(x => $"{x.LastCommit.Item1} {x.LastCommit.Item2}".Length) - 6;

        if (options.ContainsKey("quite") || options.ContainsKey("q"))
        {
            PrintOnlyBranchNames(branchTable);
            return;
        }

        PrintHeaders(longestBranchName, maxLastCommitWidth);
        PrintBranchRows(branchTable, longestBranchName, maxLastCommitWidth);
    }

    private static void PrintOnlyBranchNames(List<BranchTableRow> branchTable)
    {
        Console.WriteLine();

        foreach (BranchTableRow branch in branchTable)
        {
            if (branch.IsWorkingBranch)
            {
                Console.Write("* ");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine(branch.BranchName);
                Console.ResetColor();

                continue;
            }

            Console.WriteLine($"  {branch.BranchName}");
        }
    }

    private static void PrintHeaders(int longestBranchName, int maxLastCommitWidth)
    {
        string branchHeader = $" Branch Name  ".PadRight(longestBranchName + 1);
        string underline = new string('-', longestBranchName + 1);
        string[] headers = { " Ahead 󰜘 ", " Behind 󰜘 ", branchHeader, " Last commit  " };

        Console.WriteLine();
        PrintColoredLine(headers, ConsoleColor.Yellow);
        Console.ResetColor();
        Console.WriteLine(
            $"\n--------- | ---------- | {underline} | {new string('-', maxLastCommitWidth + 10)}"
        );
    }

    private static void PrintBranchRows(
        List<BranchTableRow> branchTable,
        int longestBranchName,
        int maxLastCommitWidth
    )
    {
        foreach (var branch in branchTable)
        {
            PrintBranchRow(branch, longestBranchName, maxLastCommitWidth);
        }
    }

    private static void PrintBranchRow(
        BranchTableRow branch,
        int longestBranchName,
        int maxLastCommitWidth
    )
    {
        var aHead = branch.Ahead.ToString().PadRight(8);
        var behind = branch.Behind.ToString().PadRight(9);
        var branchName = branch.BranchName.PadRight(longestBranchName);
        var lastCommitText =
            $"{branch.LastCommit.Item1.PadRight(maxLastCommitWidth)} {branch.LastCommit.Item2}";

        Console.Write($" {aHead} |  {behind} |  ");

        if (branch.IsWorkingBranch)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
        }
        Console.Write($"{branchName}");
        Console.ResetColor();

        Console.WriteLine($" |  {lastCommitText}     {branch.Description}");
    }

    private static void PrintColoredLine(string[] texts, ConsoleColor color)
    {
        for (int i = 0; i < texts.Length; i++)
        {
            Console.ForegroundColor = color;
            Console.Write(texts[i]);
            Console.ResetColor();
            if (i < texts.Length - 1)
            {
                Console.Write(" | ");
            }
        }
    }
}
