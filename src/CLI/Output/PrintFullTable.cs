using System.Globalization;
using Shared.TableData;

namespace Bbranch.Output;

public class PrintFullTable
{

    public static void Print(List<GitBranch> branches, int? top)
    {
        const int minimumBranchNameWidth = 14;

        if (branches.Count == 0)
        {
            Console.WriteLine("No branches found");
            return;
        }

        int longestBranchName = Math.Max(
            minimumBranchNameWidth,
            branches.Max(x => x.Branch.Name.Length + 1)
        );

        PrintHeaders(longestBranchName);
        PrintBranchRows(branches, longestBranchName, top);
    }

    private static void PrintHeaders(int longestBranchName)
    {
        string branchHeader = $" Branch Name  ".PadRight(longestBranchName + 1);
        string underline = new('-', longestBranchName + 1);
        string[] headers = [ " Ahead 󰜘 ", " Behind 󰜘 ", branchHeader, " Last commit  " ];

        PrintColoredLine(headers, ConsoleColor.Yellow);

        Console.WriteLine(
            $"\n--------- | ---------- | {underline} | {new string('-', + 15)}"
        );
    }

    private static void PrintBranchRows(
        List<GitBranch> branchTable,
        int longestBranchName,
        int? top
    )
    {
        int count = 0;

        foreach (var branch in branchTable)
        {
            if (top.HasValue && count == top)
            {
                break;
            }

            count++;

            PrintBranchRow(branch, longestBranchName);
        }
    }

    private static void PrintBranchRow(
        GitBranch branch,
        int longestBranchName
    )
    {
        var aHead = branch.AheadBehind.Ahead.ToString().PadRight(8);
        var behind = branch.AheadBehind.Behind.ToString().PadRight(9);
        var branchName = branch.Branch.Name.PadRight(longestBranchName);
        var lastCommitText = GetTimePrefix(branch.LastCommit);

        Console.Write($" {aHead} |  {behind} |  ");

        if (branch.Branch.IsWorkingBranch)
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

        Console.ResetColor();
    }

    private static string GetTimePrefix(DateTime lastCommit)
    {
        int days = (DateTime.Now - lastCommit).Days;

        if (days == 0)
        {
            string time = lastCommit.ToString("HH:mm", CultureInfo.InvariantCulture);
            return $"{time} Today";
        }

        string timeElapsed = days == 1 ? "Day" : "Days";
        return $"{days} {timeElapsed} ago";
    }
}