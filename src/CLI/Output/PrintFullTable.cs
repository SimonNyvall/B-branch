using Bbranch.Shared.TableData;

namespace Bbranch.CLI.Output;

public static class PrintFullTable
{
    public static int ScrollPosition { get => Pager.ScrollPosition; set => Pager.ScrollPosition = value; }
    private static int ConsoleHeight => Console.WindowHeight - 1;
    private static int LongestBranchNameLength { get; set; }
    private static string? currentSearchTerm;

    public static void Print(List<GitBranch> branches, PageBehaviour pageBehaviour)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

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

        if (pageBehaviour == PageBehaviour.Paginate)
        {
            StartPaging(branches);
            return;
        }

        if (pageBehaviour == PageBehaviour.None)
        {
            PrintBranchRows(branches, null, PageBehaviour.None);
            return;
        }

        if (pageBehaviour == PageBehaviour.Auto)
        {
            if (DoesOutputFitScreen(branches.Count))
            {
                StartPaging(branches);
                return;
            }

            PrintBranchRows(branches, null, PageBehaviour.None);
        }
    }

    private static void StartPaging(List<GitBranch> branches)
    {
        Console.Clear();

        PrintHeaders();
        PrintBranchRows(branches.Take(ConsoleHeight - 2).ToList(), currentSearchTerm, PageBehaviour.Paginate);

        Pager.Start(
            SpawnWindowListenerThread,
            UpdateView,
            HandleSearch,
            branches
        );
    }

    private static void SpawnWindowListenerThread(List<GitBranch> branches)
    {
        int currentPosition = ScrollPosition;
        Thread resizeListenerThread = new(() => ListenForWindowResize(branches, ref currentPosition))
        {
            IsBackground = true
        };

        resizeListenerThread.Start();
    }

    private static void ListenForWindowResize(List<GitBranch> branches, ref int scrollPosition)
    {
        int previousHeight = Console.WindowHeight;

        while (true)
        {
            if (Console.WindowHeight != previousHeight)
            {
                Pager.IsAtBottom = false;
                previousHeight = Console.WindowHeight;

                Console.Clear();
                PrintHeaders();

                // Update the scroll position to the bottom branch that is visible
                if (scrollPosition + ConsoleHeight - 2 > branches.Count)
                {
                    scrollPosition = Math.Max(0, branches.Count - ConsoleHeight + 2);
                }

                UpdateView(branches, string.Empty);

                if (branches.Count < ConsoleHeight - 1)
                {
                    Pager.IsAtBottom = true;

                    for (int i = branches.Count; i < ConsoleHeight - 2; i++)
                    {
                        Console.SetCursorPosition(0, i + 2);
                        Console.Write('~');
                    }

                    Pager.PrintEndPrompt();
                }
                else
                {
                    Pager.PrintCommandPrompt();
                }
            }

            Thread.Sleep(50);
        }
    }

    private static void UpdateView(List<GitBranch> branches, string? searchTerm)
    {
        if (searchTerm is null)
        {
            currentSearchTerm = string.Empty;
        }

        Console.SetCursorPosition(0, 2);
        PrintBranchRows(branches.Skip(ScrollPosition).Take(ConsoleHeight - 2).ToList(), currentSearchTerm, PageBehaviour.Paginate);
    }

    private static void HandleSearch(List<GitBranch> branches)
    {
        Pager.PrintSearchPrompt();

        currentSearchTerm = Console.ReadLine() ?? string.Empty;

        Console.Clear();

        if (string.IsNullOrEmpty(currentSearchTerm)) return;

        Console.SetCursorPosition(0, 0);
        PrintHeaders();

        int firstMatchIndex = branches.FindIndex(x => x.Branch.Name.Contains(currentSearchTerm, StringComparison.OrdinalIgnoreCase));

        if (firstMatchIndex == -1) return;

        Pager.IsAtBottom = false;
        if (branches.Count > ConsoleHeight)
        {
            if (Math.Abs(branches.Count - ConsoleHeight - 1) > firstMatchIndex)
            {
                ScrollPosition = firstMatchIndex;
            }
            else
            {
                ScrollPosition = Math.Abs(branches.Count - ConsoleHeight);
            }
        }
        else
        {
            ScrollPosition = 0;
            firstMatchIndex = ScrollPosition;
        }

        for (int i = ScrollPosition; i < firstMatchIndex + ConsoleHeight - 2; i++)
        {
            if (branches.Count >= ConsoleHeight)
            {
                PrintBranchRowWithHighlight(branches[i], currentSearchTerm, PageBehaviour.Paginate);
                continue;
            }

            if (i < branches.Count)
            {
                PrintBranchRowWithHighlight(branches[i], currentSearchTerm, PageBehaviour.None);
            }
        }

        if (branches.Count > ConsoleHeight) return;

        Pager.IsAtBottom = true;

        for (int i = branches.Count; i < ConsoleHeight - 1; i++)
        {
            Console.SetCursorPosition(0, i + 2);
            Console.Write('~');
        }
    }

    private static void PrintHeaders()
    {
        string branchHeader = string.Empty;
        string[] headers = [];
        string underline = new('-', LongestBranchNameLength + 1);

        if (IsUserUsingNerdFonts())
        {
            branchHeader = " Branch name  ".PadRight(LongestBranchNameLength + 1);
            headers = [" Ahead 󰜘 ", " Behind 󰜘 ", branchHeader, " Last commit  "];
        }
        else
        {
            branchHeader = " Branch name ".PadRight(LongestBranchNameLength + 1);
            headers = [" Ahead   ", " Behind   ", branchHeader, " Last commit   "];
        }

        PrintColoredLine(headers, ConsoleColor.Yellow);

        Console.WriteLine(
            $"\n--------- | ---------- | {underline} | {new string('-', +15)}"
        );
    }

    private static void PrintBranchRows(List<GitBranch> branchTable, string? search, PageBehaviour pageBehaviour)
    {
        foreach (var branch in branchTable)
        {
            PrintBranchRowWithHighlight(branch, search, pageBehaviour);
        }

        if (pageBehaviour == PageBehaviour.Paginate)
        {
            for (int i = branchTable.Count; i < ConsoleHeight - 1; i++)
            {
                Console.SetCursorPosition(0, i + 2);
                Console.Write('~');
            }
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

    private static void PrintBranchRowWithHighlight(GitBranch branch, string? search, PageBehaviour pageBehaviour)
    {
        var aHead = branch.AheadBehind.Ahead.ToString().PadRight(8);
        var behind = branch.AheadBehind.Behind.ToString().PadRight(9);
        var branchName = branch.Branch.Name.PadRight(LongestBranchNameLength);
        var lastCommitText = PrintFormater.GetTimePrefix(branch.LastCommit, DateTime.Now);
        var description = branch.Description ?? string.Empty;

        if (pageBehaviour == PageBehaviour.Paginate)
        {
            Console.Write(new string(' ', Console.WindowWidth - 1) + "\r");
        }

        Console.Write(' ');
        HighlightText(aHead.ToString(), search);
        Console.Write(" |  ");
        HighlightText(behind.ToString(), search);
        Console.Write(" |  ");

        if (branch.Branch.IsWorkingBranch)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
        }

        HighlightText(branchName, search);

        Console.ResetColor();

        Console.Write(" |  ");

        HighlightText(lastCommitText, search);

        Console.Write("    ");

        HighlightText(description, search);

        Console.WriteLine();
    }

    private static void HighlightText(string text, string? search, ConsoleColor? color = null)
    {
        int matchIndex;

        if (search is null or "")
        {
            matchIndex = -1;
        }
        else
        {
            matchIndex = text.IndexOf(search, StringComparison.OrdinalIgnoreCase);
        }

        search = search?.Trim();

        if (matchIndex >= 0)
        {
            Console.Write(text[..matchIndex]);

            if (color is not null)
            {
                Console.ForegroundColor = color.Value;
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.DarkYellow;
            }

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

    private static bool DoesOutputFitScreen(int branchCount) => branchCount > ConsoleHeight;

    private static bool IsUserUsingNerdFonts()
    {
        string gitConfigPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".gitconfig");

        if (!File.Exists(gitConfigPath)) return false;

        string[] gitConfigLines = File.ReadAllLines(gitConfigPath);

        return gitConfigLines.Any(line => line.Contains("\tuseNerdFonts = true"));
    }
}