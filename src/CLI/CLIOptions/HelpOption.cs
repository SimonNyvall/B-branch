namespace CLI.Options;

public class HelpOptions
{
    public static List<(string LongName, string ShortName, string Argument, string Description)> ValidOptionArguments { get; } =
        [
            (
                "help", 
                "h", 
                string.Empty, 
                "Displays the help message with information about all available options."
            ),
            (
                "track",
                "t",
                "<String>",
                "Displays information about how many commits the specified branch is ahead or behind relative to its upstream branch"
            ),
            (
                "sort",
                "s",
                "<String>",
                "Sorts the branches based on the specified criterion. Valid options are [date], [name], [ahead], or [behind]"
            ),
            (
                "contains",
                "c",
                "<String> OR \"String1;String2\"",
                "Filters the list to only show branches that contain the specified string"
            ),
            (
                "no-contains",
                "n",
                "<String> OR \"String1;String2\"",
                "Filters the list to only show branches that do not contain the specified string"
            ),
            (
                "all", 
                "a", 
                string.Empty,
                "Displays all branches, both local and remote"
            ),
            (
                "remote", 
                "r", 
                string.Empty,
                "Includes remote branches in the output"
            ),
            (
                "quiet",
                "q",
                string.Empty,
                "Only displays the names of the branches without any additional information or formatting"
            ),
            (
                "print-top", 
                "p", 
                "<Number>",
                "Prints the top N branches based on the specified sort criterion"
            ),
            (
                "version", 
                "v", 
                string.Empty,
                "Shows the current version of the tool"
            )
        ];

    public static void Execute()
    {
        Console.WriteLine("Usage: bbranch [options]\n");
        Console.WriteLine("Options:");

        foreach ((string longName, string shortName,string argument, string description) in ValidOptionArguments)
        {
            Console.Write($"  --{longName}, ");

            Console.CursorLeft = 17;
            Console.Write($"-{shortName} ");

            PrintAruement(argument);

            PrintDescription(description);
        }

        Environment.Exit(0);
    }

    private static void PrintAruement(string argument)
    {
        if (argument == string.Empty) return;

        foreach (char letter in argument)
        {
            if 
            (
                letter == '<' || 
                letter == '>' ||
                letter == '"' ||
                letter == ';'
            )
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(letter);
                continue;
            }

            Console.ResetColor();
            Console.Write(letter);
        }        

        Console.ResetColor();
    }

    private static void PrintDescription(string description)
    {
        Console.CursorLeft = 50;

        foreach (char letter in description)
        {
            if (letter == '[')
            {
                Console.Write(letter);
                Console.ForegroundColor = ConsoleColor.Yellow;
                continue;
            }

            if (letter == ']')
            {
                Console.ResetColor();
            }

            Console.Write(letter);
        }

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine();

        Console.ResetColor();
    }
}
