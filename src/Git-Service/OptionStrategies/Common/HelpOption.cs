namespace Git.Options;

// This should not implement IOption and should be moved to a different place
public class HelpOptions
{
    // long Argument, short argument, description
    public static List<(string, string, string)> ValidArgs { get; } =
        [
            ("help", "h", "Displays the help message"),
            (
                "track",
                "t",
                "Displays information about how many commits the specified branch is ahead or behind relative to its upstream branch"
            ),
            (
                "sort",
                "s",
                "Sorts the branches based on the specified criterion. Valid options are [date], [name], [ahead], or [behind]"
            ),
            (
                "contains",
                "c",
                "Filters the list to only show branches that contain the specified string"
            ),
            (
                "no-contains",
                "n",
                "Filters the list to only show branches that do not contain the specified string"
            ),
            ("all", "a", "Displays all branches, both local and remote"),
            ("remote", "r", "Includes remote branches in the output"),
            (
                "quiet",
                "q",
                "Only displays the names of the branches without any additional information or formatting"
            ),
            ("print-top", "p", "Prints the top N branches based on the specified sort criterion"),
            ("version", "v", "Shows the current version of the tool")
        ];

    public static void Execute()
    {
        Console.WriteLine("Usage: bbranch [options]\n");
        Console.WriteLine("Options:");
        foreach ((string longArg, string shortArg, string description) in ValidArgs)
        {
            Console.Write($"  --{longArg}, ");

            Console.CursorLeft = 17;
            Console.Write($"-{shortArg}: ");

            Console.CursorLeft = 22;
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(description);
            Console.ResetColor();
        }

        Environment.Exit(0);
    }
}
