using Bbranch.Shared.TableData;

namespace Bbranch.CLI.Output;

public static class PrintLightTable
{
    public static int ScrollPosition { get => Pager.ScrollPosition; set => Pager.ScrollPosition = value; }
    private static int ConsoleHeight => Console.WindowHeight - 1;
    private static string? currentSearchTerm;

    public static void Print(List<GitBranch> branches, PageBehaviour pageBehaviour)
    {
        if (branches.Count == 0)
        {
            Console.WriteLine("No branches found");
            return;
        }


        if (pageBehaviour == PageBehaviour.Paginate)
        {
            StartPaging(branches);
            return;
        }

        if (pageBehaviour == PageBehaviour.None)
        {
            PrintBranches(branches, null, PageBehaviour.None);
            return;
        }

        if (pageBehaviour == PageBehaviour.Auto)
        {
            if (DoesOutputFitScreen(branches.Count))
            {
                StartPaging(branches);
                return;
            }

            PrintBranches(branches, null, PageBehaviour.None);
        }
    }

    private static void StartPaging(List<GitBranch> branches)
    {
        Console.Clear();

        PrintBranches(branches.Take(ConsoleHeight).ToList(), currentSearchTerm, PageBehaviour.Paginate);

        Pager.HeaderHight = 0;

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
        Thread resizeListenerThread = new(() => ListenForWindowResize(branches))
        {
            IsBackground = true
        };

        resizeListenerThread.Start();
    }

    private static void ListenForWindowResize(List<GitBranch> branches)
    {
        int previousHeight = Console.WindowHeight;

        while (true)
        {
            if (Console.WindowHeight != previousHeight)
            {
                Pager.IsAtBottom = false;

                previousHeight = Console.WindowHeight;
                Console.Clear();

                if (ScrollPosition + ConsoleHeight > branches.Count)
                {
                    ScrollPosition = Math.Max(0, branches.Count - ConsoleHeight);
                }

                UpdateView(branches, string.Empty);

                if (branches.Count < ConsoleHeight - 1)
                {
                    for (int i = branches.Count; i < ConsoleHeight; i++)
                    {
                        Console.SetCursorPosition(0, i);
                        Console.Write('~');
                    }

                    Pager.PrintEndPrompt();
                    Pager.IsAtBottom = true;
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

        Console.SetCursorPosition(0, 0);
        PrintBranches(branches.Skip(ScrollPosition).Take(ConsoleHeight).ToList(), currentSearchTerm, PageBehaviour.Paginate);
    }

    private static void HandleSearch(List<GitBranch> branches)
    {
        Pager.PrintSearchPrompt();

        currentSearchTerm = Console.ReadLine() ?? string.Empty;

        Console.Clear();

        if (string.IsNullOrEmpty(currentSearchTerm)) return;

        Console.SetCursorPosition(0, 0);

        int firstMatchIndex = branches.FindIndex(x => x.Branch.Name.Contains(currentSearchTerm, StringComparison.OrdinalIgnoreCase));

        if (firstMatchIndex == -1) return;

        Pager.IsAtBottom = false;
        if (branches.Count > ConsoleHeight)
        {
            if (Math.Abs(ConsoleHeight - branches.Count - 1) > firstMatchIndex)
            {
                ScrollPosition = firstMatchIndex;
            }
            else
            {
                ScrollPosition = Math.Abs(branches.Count - ConsoleHeight);
                firstMatchIndex = ScrollPosition;
            }
        }
        else
        {
            ScrollPosition = 0;
            firstMatchIndex = ScrollPosition;
        }

        for (int i = ScrollPosition; i < firstMatchIndex + ConsoleHeight; i++)
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

        for (int i = branches.Count; i < ConsoleHeight; i++)
        {
            Console.SetCursorPosition(0, i);
            Console.Write('~');
        }
    }

    private static bool DoesOutputFitScreen(int branchCount) => branchCount > ConsoleHeight;

    private static void PrintBranches(List<GitBranch> branches, string? search, PageBehaviour pageBehaviour)
    {
        foreach (var branch in branches)
        {
            PrintBranchRowWithHighlight(branch, search, pageBehaviour);
        }

        if (pageBehaviour == PageBehaviour.Paginate)
        {
            for (int i = branches.Count; i < ConsoleHeight; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write('~');
            }
        }
    }

    private static void PrintBranchRowWithHighlight(GitBranch branch, string? search, PageBehaviour pageBehaviour)
    {
        if (pageBehaviour == PageBehaviour.Paginate)
        {
            Console.Write(new string(' ', Console.WindowWidth - 1) + '\r');
        }

        if (branch.Branch.IsWorkingBranch)
        {
            Console.Write("* ");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
        }
        else
        {
            Console.Write("  ");
        }

        HighlightText(branch.Branch.Name, search);

        Console.ResetColor();
    }

    private static void HighlightText(string text, string? search)
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

        try
        {
            Console.Write(new string(' ', Console.WindowWidth - text.Length - 1) + '\r');
        }
        catch
        {
            // ignored
        }
    }
}