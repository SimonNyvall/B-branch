using Bbranch.Shared.TableData;

namespace Bbranch.CLI.Output;

internal class Pager
{
    public static int ScrollPosition { get => _scrollPosition; set => _scrollPosition = value; }
    public static bool IsAtBottom { get; set; } = false;
    public static int HeaderHight { get; set; } = 2;
    private static int _scrollPosition = 0;
    private static int ConsoleHeight => Console.WindowHeight - 1;

    public static void Start(
        Action<List<GitBranch>> windowResize,
        Action<List<GitBranch>, string?> updateView,
        Action<List<GitBranch>> searchHandler,
        List<GitBranch> branches)
    {
        Console.CursorVisible = false;

        windowResize(branches);

        while (true)
        {
            Console.SetCursorPosition(0, ConsoleHeight);

            if (IsScrollAtBottom(_scrollPosition, branches.Count))
            {
                PrintEndPrompt();
                IsAtBottom = true;
            }
            else
            {
                PrintCommandPrompt();
            }

            ConsoleKeyInfo key = Console.ReadKey(true);

            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                case ConsoleKey.K:
                    {
                        if (!CanScrollUp(_scrollPosition)) break;

                        _scrollPosition--;
                        updateView(branches, string.Empty);
                        IsAtBottom = false;
                        break;
                    }
                case ConsoleKey.DownArrow:
                case ConsoleKey.J:
                    {
                        if (!CanScrollDown(_scrollPosition, branches.Count)) break;

                        _scrollPosition++;
                        updateView(branches, string.Empty);
                        break;
                    }
                case ConsoleKey.Home:
                case ConsoleKey.End:
                case ConsoleKey.G:
                    {
                        if (key.KeyChar == 'G' || key.Key == ConsoleKey.End)
                        {
                            if (!IsAtBottom)
                            {
                                IsAtBottom = true;
                                _scrollPosition = Math.Abs(branches.Count - ConsoleHeight + HeaderHight);
                                updateView(branches, string.Empty);
                            }
                            break;
                        }

                        if (key.KeyChar == 'g' || key.Key == ConsoleKey.Home)
                        {
                            _scrollPosition = 0;
                            IsAtBottom = false;
                            updateView(branches, string.Empty);
                            break;
                        }

                        break;
                    }
                case ConsoleKey.F:
                case ConsoleKey.Spacebar:
                    {
                        int pageHeight = ConsoleHeight - 2;

                        if (CanPageDown(_scrollPosition, branches.Count))
                        {
                            _scrollPosition += pageHeight;
                        }
                        else
                        {
                            _scrollPosition = branches.Count - pageHeight;
                        }

                        updateView(branches, string.Empty);
                        break;
                    }
                case ConsoleKey.B:
                    {
                        if (CanPageUp(_scrollPosition))
                        {
                            _scrollPosition -= ConsoleHeight;
                        }
                        else
                        {
                            _scrollPosition = 0;
                        }

                        updateView(branches, string.Empty);
                        break;
                    }
                case ConsoleKey.Divide:
                case ConsoleKey.Oem2:
                case ConsoleKey.D7:
                    searchHandler(branches);
                    break;
                case ConsoleKey.Escape:
                    {
                        updateView(branches, null);
                        break;
                    }
                case ConsoleKey.Q:
                    Console.CursorVisible = true;
                    return;
            }
        }
    }

    internal static void PrintCommandPrompt()
    {
        Console.SetCursorPosition(0, ConsoleHeight);
        Console.Write(new string(' ', Console.WindowWidth - 1) + '\r');
        Console.Write(":");
    }

    internal static void PrintEndPrompt()
    {
        Console.SetCursorPosition(0, ConsoleHeight);
        Console.Write(new string(' ', Console.WindowWidth - 1) + '\r');
        Console.BackgroundColor = ConsoleColor.White;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.Write("(END)");
        Console.ResetColor();
    }

    internal static void PrintSearchPrompt()
    {
        Console.SetCursorPosition(0, ConsoleHeight);
        Console.Write(new string(' ', Console.WindowWidth - 1) + '\r');
        Console.Write("/");
        Console.SetCursorPosition(1, ConsoleHeight);
    }

    private static bool IsScrollAtBottom(int scrollPosition, int branchCount, int offset = 0) =>
        scrollPosition + ConsoleHeight == branchCount + 2 || branchCount < ConsoleHeight - offset;

    private static bool CanScrollUp(int scrollPosition) => scrollPosition > 0;

    private static bool CanScrollDown(int scrollPosition, int branchCount) =>
        scrollPosition < Math.Abs(branchCount - ConsoleHeight + HeaderHight) && !IsAtBottom;

    private static bool CanPageDown(int scrollPosition, int branchCount) =>
        branchCount - scrollPosition - (ConsoleHeight - 2) > (ConsoleHeight - 2);

    private static bool CanPageUp(int scrollPosition) => scrollPosition - ConsoleHeight > 0;
}

public enum PageBehaviour
{
    None,
    Paginate,
    Auto
}