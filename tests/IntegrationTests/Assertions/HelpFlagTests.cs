namespace Bbranch.IntegrationTests;

[Collection("Sequential")]
public class HelpFlagTests : IntegrationBase
{
    private readonly string _helpMessage = """
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
    """.Replace("\r", "").Replace("\n", "");

    [Fact(Timeout = 120000)]
    public async Task IntegrationTest_ValidOutput_WithHelpShortFlag()
    {
        using var process = GetBbranchProcess("-h");

        var (output, error) = await RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        output = output.Replace("\r", "").Replace("\n", "");

        Assert.Equal(_helpMessage, output);
    }

    [Fact(Timeout = 120000)]
    public async Task IntegrationTest_ValidOutput_WithHelpLongFlag()
    {
        using var process = GetBbranchProcess("--help");

        var (output, error) = await RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        output = output.Replace("\r", "").Replace("\n", "");

        Assert.Equal(_helpMessage, output);
    }

    [Fact(Timeout = 120000)]
    public async Task IntegrationTest_InvalidOutput_WithHelpFlagAndValue()
    {
        using var process = GetBbranchProcess("--help", "value");

        var (output, error) = await RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        output = output.Replace("\r", "");

        Assert.Equal("fatal: Value for --help is not allowed\n", output);
    }
}