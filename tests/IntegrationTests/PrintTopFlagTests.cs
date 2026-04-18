namespace Bbranch.IntegrationTests;

[Collection(Constants.DefaultFixtureName)]
public class PrintTopFlagTests
{
    private readonly DefaultFixture _fixture;

    public PrintTopFlagTests(DefaultFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task IntegrationTest_ValidOutput_WithPrintTopLongFlag()
    {
        using var process = _fixture.GetBbranchProcess("--print-top", "1");

        var (output, error) = await _fixture.RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        string[] lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        _fixture.AssertHeader(lines);

        Assert.True(lines.Length <= 3, $"Too many lines printed... Actual: {lines.Length}");
    }

    [Fact]
    public async Task IntegrationTest_ValidOutput_WithPrintTopShortFlag()
    {
        using var process = _fixture.GetBbranchProcess("-p", "1");

        var (output, error) = await _fixture.RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        string[] lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        _fixture.AssertHeader(lines);

        Assert.True(lines.Length <= 3, $"Too many lines printed... Actual: {lines.Length}");
    }

    [Fact]
    public async Task IntegrationTest_InvalidOutput_WithPrintTopFlagAndInvalidValue()
    {
        using var process = _fixture.GetBbranchProcess("--print-top", "value");

        var (output, error) = await _fixture.RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        output = output.Replace("\r", "");

        Assert.Equal("fatal: Value for --print-top must be an integer\n", output);
    }

    [Fact]
    public async Task IntegrationTest_InvalidOutput_WithPrintTopFlagAndZeroValue()
    {
        using var process = _fixture.GetBbranchProcess("--print-top", "0");

        var (output, error) = await _fixture.RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        output = output.Replace("\r", "");

        Assert.Equal("fatal: Value for --print-top must be greater than 0\n", output);
    }

    [Fact]
    public async Task IntegrationTest_InvalidOutput_WithPrintTopFlagAndNoValue()
    {
        using var process = _fixture.GetBbranchProcess("--print-top");

        var (output, error) = await _fixture.RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        output = output.Replace("\r", "");

        Assert.Equal("fatal: Value for --print-top is missing\n", output);
    }
}
