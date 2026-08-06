using Bbranch.Tests.Infrastucture.Fixtures;

namespace Bbranch.Tests.Integration;

[Trait("Category", "Integration")]
public class HelpFlagTests
{
    private readonly TestExecutor _sut;

    public HelpFlagTests(DefaultFixture fixture)
    {
        _sut = new TestExecutor(fixture);
    }

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
        """.Replace("\r", "").Replace("\n", "");

    [Fact]
    public async Task IntegrationTest_ValidOutput_WithHelpShortFlag()
    {
        var result = await _sut.ExecuteWithShortFlag();

        result = result.Replace("\n", "");

        Assert.Equal(_helpMessage, result);
    }

    [Fact]
    public async Task IntegrationTest_ValidOutput_WithHelpLongFlag()
    {
        var result = await _sut.ExecuteWithLongFlag();

        result = result.Replace("\n", "");

        Assert.Equal(_helpMessage, result);
    }

    [Fact]
    public async Task IntegrationTest_InvalidOutput_WithHelpFlagAndValue()
    {
        var result = await _sut.ExecuteWithValue();

        Assert.Equal("fatal: Value for --help is not allowed\n", result);
    }

    private class TestExecutor(DefaultFixture fixture)
    {
        public async Task<string> ExecuteWithShortFlag() => await fixture.Exec("-h");

        public async Task<string> ExecuteWithLongFlag() => await fixture.Exec("--help");

        public async Task<string> ExecuteWithValue() => await fixture.Exec("--help", "value");
    }
}
