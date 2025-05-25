namespace Bbranch.CLI.Options;

public static class HelpOption
{
    private static readonly string HelpMessage = """
    usage: git bb [<options>] [<additional arguments>]

    Generic options:
        -h, --help              Show the help message.
        -v, --version           Show the current version of the tool.
        -q, --quiet             Suppress additional output, showing only branch names.

    Filtering options:
        -c, --contains <string>         List brnches containing the specified string.
        -n, --no-contains <string>      List branches not containing the specified string.
        -s, --sort <criterion>          Sort branches by <date|name|ahead|behind>.
        -t, --track <branch>            Show upstream relationship of the specified branch.
        -a, --all                       List both local and remote branches.
        -r, --remote                    List only remote branches.
        -p, --print-top <N>             Show the top N branches based on sort criterion.

    Display options:
        --pager                         Force output to display in a pager.
        --no-pager                      Display output directly in the console.
    """;

    public static void Execute()
    {
        Console.WriteLine(HelpMessage);
        Environment.Exit(0);
    }
}
