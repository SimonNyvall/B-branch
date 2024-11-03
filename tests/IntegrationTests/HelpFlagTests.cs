namespace Bbranch.IntegrationTests;

public class HelpFlagTests
{
    [Fact]
    public async Task IntegrationTest_ValidOutput_WithHelpFlag()
    {
        await IntegrationTest_ValidOutput_WithHelpShortFlag();
        await IntegrationTest_ValidOutput_WithHelpLongFlag();
    }

    private static async Task IntegrationTest_ValidOutput_WithHelpShortFlag()
    {
        using var process = ProcessHelper.GetDotnetProcess("-h");
        process.Start();

        string output = await process.StandardOutput.ReadToEndAsync();
        string error = await process.StandardError.ReadToEndAsync();

        Assert.True(string.IsNullOrEmpty(error), error);

        string[] lines = output.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

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

    private static async Task IntegrationTest_ValidOutput_WithHelpLongFlag()
    {
        using var process = ProcessHelper.GetDotnetProcess("--help");
        process.Start();

        string output = await process.StandardOutput.ReadToEndAsync();
        string error = await process.StandardError.ReadToEndAsync();

        Assert.True(string.IsNullOrEmpty(error), error);

        string[] lines = output.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

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