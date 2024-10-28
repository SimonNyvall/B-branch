using Bbranch.Shared.TableData;

namespace Bbranch.CLI.Output;

internal class Pager
{
    private static int _scrollPosition = 0;
    private static string? _currentSearchTerm = null;
    private static int ConsoleHeight => Console.WindowHeight - 1;

    public static void Start(
        Action<List<GitBranch>, int> windowResize,
        Action<List<GitBranch>, int> updateView,
        Func<List<GitBranch>, int, int> searchHandler,
        List<GitBranch> branches)
    {
        Console.CursorVisible = false;

        windowResize(branches, _scrollPosition);

        while (true)
        {
            Console.SetCursorPosition(0, ConsoleHeight);

            if (IsScrollAtBottom(_scrollPosition, branches.Count))
            {
                PrintEndPromt();
            }
            else
            {
                PrintCommandPromt();
            }

            ConsoleKeyInfo key = Console.ReadKey(true);

            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                case ConsoleKey.K:
                    {
                        if (!CanScrollUp(_scrollPosition)) break;

                        _scrollPosition--;
                        updateView(branches, _scrollPosition);
                        break;
                    }
                case ConsoleKey.DownArrow:
                case ConsoleKey.J:
                    {
                        if (!CanScrollDown(_scrollPosition, branches.Count)) break;

                        _scrollPosition++;
                        updateView(branches, _scrollPosition);
                        break;
                    }
                case ConsoleKey.Home:
                case ConsoleKey.End:
                case ConsoleKey.G:
                    {
                        if (key.KeyChar == 'G' || key.Key == ConsoleKey.End)
                        {
                            _scrollPosition = Math.Abs(branches.Count - ConsoleHeight + 2);
                            updateView(branches, _scrollPosition);
                            break;
                        }

                        if (key.KeyChar == 'g' || key.Key == ConsoleKey.Home)
                        {
                            _scrollPosition = 0;
                            updateView(branches, _scrollPosition);
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

                        updateView(branches, _scrollPosition);
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

                        updateView(branches, _scrollPosition);
                        break;
                    }
                case ConsoleKey.Divide:
                case ConsoleKey.Oem2:
                case ConsoleKey.D7:
                    _scrollPosition = searchHandler(branches, _scrollPosition);
                    break;
                case ConsoleKey.Escape:
                    {
                        _currentSearchTerm = null;
                        updateView(branches, _scrollPosition);
                        break;
                    }
                case ConsoleKey.Q:
                    Console.CursorVisible = true;
                    return;
            }
        }
    }

    private static void PrintCommandPromt()
    {
        Console.SetCursorPosition(0, ConsoleHeight);
        Console.Write(":    ");
    }

    private static void PrintEndPromt()
    {
        Console.SetCursorPosition(0, ConsoleHeight);
        Console.BackgroundColor = ConsoleColor.White;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.Write("(END)");
        Console.ResetColor();
    }

    private static void PrintSearchPromt()
    {
        Console.SetCursorPosition(0, ConsoleHeight);
        Console.Write("/    ");
        Console.SetCursorPosition(1, ConsoleHeight);
    }

    private static bool IsScrollAtBottom(int scrollPosition, int branchCount, int offset = 0) =>
        scrollPosition + ConsoleHeight == branchCount + 2 || branchCount < ConsoleHeight - offset;

    private static bool CanScrollUp(int scrollPosition) => scrollPosition > 0;

    private static bool CanScrollDown(int scrollPosition, int branchCount) =>
        scrollPosition < Math.Abs(branchCount - ConsoleHeight + 2);

    private static bool CanPageDown(int scrollPosition, int branchCount) =>
        branchCount - scrollPosition - (ConsoleHeight - 2) > (ConsoleHeight - 2);

    private static bool CanPageUp(int scrollPosition) => scrollPosition - ConsoleHeight > 0;
}