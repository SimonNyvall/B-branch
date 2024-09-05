using System.Globalization;
using Shared.TableData;

namespace CLI.Output;

public class PrintFullTable
{
    private static int ConsoleHeight => Console.WindowHeight - 1;
    private static int LongestBranchNameLength { get; set; }
    private static string? currentSearchTerm;

    public static void Print(List<GitBranch> branches)
    {
        const int minimumBranchNameWidth = 14;

        if (branches.Count == 0)
        {
            Console.WriteLine("No branches found");
            return;
        }

        LongestBranchNameLength = Math.Max(
            minimumBranchNameWidth,
            branches.Max(x => x.Branch.Name.Length + 1)
        );

        PrintHeaders();

        if (Console.IsOutputRedirected || !Console.IsInputRedirected)
        {
            PrintBranchRows(branches, null);
            return;
        }

        if (branches.Count > ConsoleHeight)
        {
            StartPaging(branches);

            return;
        }

        PrintBranchRows(branches, null);
    }

    private static void StartPaging(List<GitBranch> branches)
    {
        Console.CursorVisible = false;
        int scrollPosition = 0;
        currentSearchTerm = null;

        Console.Clear();
        PrintHeaders();
        PrintBranchRows(branches.Take(ConsoleHeight - 2).ToList(), currentSearchTerm);

        while (true)
        {
            Console.SetCursorPosition(0, ConsoleHeight);
            Console.Write(':');
            ConsoleKeyInfo key = Console.ReadKey(true);

            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                case ConsoleKey.K:
                    {
                        if (scrollPosition <= 0) break;

                        scrollPosition--;
                        UpdateView(branches, scrollPosition);
                        break;
                    }
                case ConsoleKey.DownArrow:
                case ConsoleKey.J:
                    {
                        if (scrollPosition > Math.Abs(branches.Count - ConsoleHeight + 1)) break;

                        scrollPosition++;
                        UpdateView(branches, scrollPosition);
                        break;
                    }
                case ConsoleKey.Home:
                case ConsoleKey.End:
                case ConsoleKey.G:
                    {
                        if (key.KeyChar == 'G' || key.Key == ConsoleKey.End)
                        {
                            scrollPosition = Math.Abs(branches.Count - ConsoleHeight + 2);
                            UpdateView(branches, scrollPosition);
                            break;
                        }

                        if (key.KeyChar == 'g' || key.Key == ConsoleKey.Home)
                        {
                            scrollPosition = 0;
                            UpdateView(branches, scrollPosition);
                            break;
                        }

                        break;
                    }
                case ConsoleKey.F:
                case ConsoleKey.Spacebar:
                    {
                        int pageHeight = ConsoleHeight - 2;

                        if (branches.Count - scrollPosition - pageHeight > pageHeight)
                        {
                            scrollPosition += pageHeight;
                        }
                        else
                        {
                            scrollPosition = branches.Count - pageHeight;
                        }

                        UpdateView(branches, scrollPosition);
                        break;
                    }
                case ConsoleKey.B:
                    {
                        if (scrollPosition - ConsoleHeight > 0)
                        {
                            scrollPosition -= ConsoleHeight;
                        }
                        else
                        {
                            scrollPosition = 0;
                        }

                        UpdateView(branches, scrollPosition);
                        break;
                    }
                case ConsoleKey.Divide:
                    HandleSearch(branches);
                    break;
                case ConsoleKey.Escape:
                    {
                        currentSearchTerm = null;
                        UpdateView(branches, scrollPosition);
                        break;
                    }
                case ConsoleKey.Q:
                    Console.CursorVisible = true;
                    return;
            }
        }
    }

    private static void UpdateView(List<GitBranch> branches, int scrollPosition)
    {
        Console.SetCursorPosition(0, 2);
        PrintBranchRows(branches.Skip(scrollPosition).Take(ConsoleHeight - 2).ToList(), currentSearchTerm);
    }

    private static void HandleSearch(List<GitBranch> branches)
    {
        Console.SetCursorPosition(0, ConsoleHeight);
        Console.Write('/');

        currentSearchTerm = Console.ReadLine() ?? string.Empty;

        Console.Clear();

        if (string.IsNullOrEmpty(currentSearchTerm)) return;

        Console.SetCursorPosition(0, 0);
        PrintHeaders();

        for (int i = 0; i < branches.Count; i++)
        {
            if (i > ConsoleHeight - 3) break;

            PrintBranchRowWithHighlight(branches[i], currentSearchTerm);
        }
    }

    private static void PrintHeaders()
    {
        string branchHeader = " Branch Name  ".PadRight(LongestBranchNameLength + 1);
        string underline = new('-', LongestBranchNameLength + 1);
        string[] headers = [" Ahead 󰜘 ", " Behind 󰜘 ", branchHeader, " Last commit  "];

        PrintColoredLine(headers, ConsoleColor.Yellow);

        Console.WriteLine(
            $"\n--------- | ---------- | {underline} | {new string('-', +15)}"
        );
    }

    private static void PrintBranchRows(List<GitBranch> branchTable, string? search)
    {
        foreach (var branch in branchTable)
        {
            PrintBranchRowWithHighlight(branch, search);
        }
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

    private static void PrintBranchRowWithHighlight(GitBranch branch, string? search)
    {
        var aHead = branch.AheadBehind.Ahead.ToString().PadRight(8);
        var behind = branch.AheadBehind.Behind.ToString().PadRight(9);
        var branchName = branch.Branch.Name.PadRight(LongestBranchNameLength);
        var lastCommitText = GetTimePrefix(branch.LastCommit);
        var description = branch.Description ?? string.Empty;

        Console.Write($" {aHead} |  {behind} |  ");

        if (branch.Branch.IsWorkingBranch)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
        }

        HighlightText(branchName, search);

        Console.ResetColor();
        Console.Write($" |  {lastCommitText}     ");

        HighlightText(description, search);

        Console.WriteLine();
    }

    private static void HighlightText(string text, string? search)
    {
        int matchIndex;

        if (search is null)
        {
            matchIndex = -1;
        }
        else
        {
            matchIndex = text.IndexOf(search, StringComparison.OrdinalIgnoreCase);
        }

        if (matchIndex >= 0)
        {
            Console.Write(text[..matchIndex]);

            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(text.Substring(matchIndex, search!.Length));

            Console.ResetColor();
            Console.Write(text[(matchIndex + search.Length)..]);
        }
        else
        {
            Console.Write(text);
        }
    }

    private static string GetTimePrefix(DateTime lastCommit)
    {
        int days = (DateTime.Now - lastCommit).Days;

        if (days == 0)
        {
            string time = lastCommit.ToString("HH:mm", CultureInfo.InvariantCulture);
            return $"{time} Today";
        }

        string timeElapsed = days == 1 ? "day" : "days";

        int padLeft = 5 - days.ToString().Length;
        return $"{days} {new string(' ', padLeft)}{timeElapsed} ago";
    }
}