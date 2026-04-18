namespace Bbranch.IntegrationTests;

[Collection("Sequential")]
public class PrintTopFlagTests : IntegrationBase
{
    [Fact(Timeout = 120000)]
    public async Task IntegrationTest_ValidOutput_WithPrintTopLongFlag()
    {
        using var process = GetBbranchProcess("--print-top", "1");

        var (output, error) = await RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        string[] lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        AssertHeader(lines);

        Assert.True(lines.Length <= 3, $"Too many lines printed... Actual: {lines.Length}");
    }

    [Fact(Timeout = 120000)]
    public async Task IntegrationTest_ValidOutput_WithPrintTopShortFlag()
    {
        using var process = GetBbranchProcess("-p", "1");

        var (output, error) = await RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        string[] lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        AssertHeader(lines);

        Assert.True(lines.Length <= 3, $"Too many lines printed... Actual: {lines.Length}");
    }

    [Fact(Timeout = 120000)]
    public async Task IntegrationTest_InvalidOutput_WithPrintTopFlagAndInvalidValue()
    {
        using var process = GetBbranchProcess("--print-top", "value");

        var (output, error) = await RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        output = output.Replace("\r", "");

        Assert.Equal("fatal: Value for --print-top must be an integer\n", output);
    }

    [Fact(Timeout = 120000)]
    public async Task IntegrationTest_InvalidOutput_WithPrintTopFlagAndZeroValue()
    {
        using var process = GetBbranchProcess("--print-top", "0");

        var (output, error) = await RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        output = output.Replace("\r", "");

        Assert.Equal("fatal: Value for --print-top must be greater than 0\n", output);
    }

    [Fact(Timeout = 120000)]
    public async Task IntegrationTest_InvalidOutput_WithPrintTopFlagAndNoValue()
    {
        using var process = GetBbranchProcess("--print-top");

        var (output, error) = await RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        output = output.Replace("\r", "");

        Assert.Equal("fatal: Value for --print-top is missing\n", output);
    }
}
