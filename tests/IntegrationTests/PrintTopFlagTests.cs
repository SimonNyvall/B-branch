namespace Bbranch.IntegrationTests;

[Collection("Sequential")]
public class PrintTopFlagTests : IntegrationBase
{
    [Fact(Timeout = 120000)]
    public async Task IntegrationTest_ValidOutput_WithPrintTopLongFlag()
    {
        using var process = GetBbranchProcessWithoutPager("--print-top", "1");
       
        var (output, error) = await RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        string[] lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        AssertHeader(lines);

        Assert.True(lines.Length <= 3, $"Too many lines printed... Actual: {lines.Length}");
    }

    [Fact(Timeout = 120000)]
    public async Task IntegrationTest_ValidOutput_WithPrintTopShortFlag()
    {
        using var process = GetBbranchProcessWithoutPager("-p", "1");

        var (output, error) = await RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        string[] lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        AssertHeader(lines);

        Assert.True(lines.Length <= 3, $"Too many lines printed... Actual: {lines.Length}");
    }
}