namespace CLI.Options;

public class HelpOptions
{
    private static string _helpMessage = """
    Usage: git bb [options]

    Options:
    --help,        -h                               Displays the help message with information about all available options.
    --track,       -t <String>                      Displays information about how many commits the specified branch is ahead or behind relative to its upstream branch
    --sort,        -s <String>                      Sorts the branches based on the specified criterion. Valid options are [date], [name], [ahead], or [behind]
    --contains,    -c <String> OR "String1;String2" Filters the list to only show branches that contain the specified string
    --no-contains, -n <String> OR "String1;String2" Filters the list to only show branches that do not contain the specified string
    --all,         -a                               Displays all branches, both local and remote
    --remote,      -r                               Includes remote branches in the output
    --quiet,       -q                               Only displays the names of the branches without any additional information or formatting
    --print-top,   -p <Number>                      Prints the top N branches based on the specified sort criterion
    --version,     -v                               Shows the current version of the tool
    """;

    public static void Execute()
    {
        Console.WriteLine(_helpMessage);
        Environment.Exit(0);
    }
}
