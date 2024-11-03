namespace Bbranch.IntegrationTests;

public class PrintTopFlagTests : IntegrationBase
{
    [Fact(Timeout = 120000)]
    public async Task IntegrationTest_ValidOutput_WithPrintTopFlag()
    {
        await IntegrationTest_ValidOutput_WithPrintTopShortFlag();
        await IntegrationTest_ValidOutput_WithPrintTopLongFlag();
    }

    private async Task IntegrationTest_ValidOutput_WithPrintTopLongFlag()
    {
        using var process = GetDotnetProcess("--print-top", "1");
       
        var (output, error) = await RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        string[] lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        AssertHeader(lines);

        Assert.True(lines.Length <= 3, $"Too many lines printed... Actual: {lines.Length}");
    }

    private async Task IntegrationTest_ValidOutput_WithPrintTopShortFlag()
    {
        using var process = GetDotnetProcess("-p", "1");

        var (output, error) = await RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        string[] lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        AssertHeader(lines);

        Assert.True(lines.Length <= 3, $"Too many lines printed... Actual: {lines.Length}");
    }
}