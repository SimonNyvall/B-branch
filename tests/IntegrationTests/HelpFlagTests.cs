namespace Bbranch.IntegrationTests;

[Collection("Sequential")]
public class HelpFlagTests : IntegrationBase
{
    [Fact(Timeout = 120000)]
    public async Task IntegrationTest_ValidOutput_WithHelpShortFlag()
    {
        using var process = GetBbranchProcess("-h");

        var (output, error) = await RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        output = output.Replace("\r", "");

        string[] lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        Assert.Contains("--help,        -h", lines[2]);
        Assert.Contains("--track,       -t", lines[3]);
        Assert.Contains("--sort,        -s", lines[4]);
        Assert.Contains("--contains,    -c", lines[5]);
        Assert.Contains("--no-contains, -n", lines[6]);
        Assert.Contains("--all,         -a", lines[7]);
        Assert.Contains("--remote,      -r", lines[8]);
        Assert.Contains("--quiet,       -q", lines[9]);
        Assert.Contains("--print-top,   -p", lines[10]);
        Assert.Contains("--version,     -v", lines[11]);
    }

    [Fact(Timeout = 120000)]
    public async Task IntegrationTest_ValidOutput_WithHelpLongFlag()
    {
        using var process = GetBbranchProcess("--help");

        var (output, error) = await RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        output = output.Replace("\r", "");

        string[] lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        Assert.Contains("--help,        -h", lines[2]);
        Assert.Contains("--track,       -t", lines[3]);
        Assert.Contains("--sort,        -s", lines[4]);
        Assert.Contains("--contains,    -c", lines[5]);
        Assert.Contains("--no-contains, -n", lines[6]);
        Assert.Contains("--all,         -a", lines[7]);
        Assert.Contains("--remote,      -r", lines[8]);
        Assert.Contains("--quiet,       -q", lines[9]);
        Assert.Contains("--print-top,   -p", lines[10]);
        Assert.Contains("--version,     -v", lines[11]);
    }
}