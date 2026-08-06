using Bbranch.Tests.Helpers;
using Bbranch.Tests.Infrastucture.Fixtures;

namespace Bbranch.Tests.Integration;

[Trait("Category", "Integration")]
public class PrintTopFlagTests
{
    private readonly TestExecutor _sut;

    public PrintTopFlagTests(DefaultFixture fixture)
    {
        _sut = new TestExecutor(fixture);
    }

    [Fact]
    public async Task IntegrationTest_ValidOutput_WithPrintTopLongFlag()
    {
        var result = await _sut.ExecuteWithLongFlag();

        string[] lines = result.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        AssertHelper.Header(lines);

        Assert.True(lines.Length <= 3, $"Too many lines printed... Actual: {lines.Length}");
    }

    [Fact]
    public async Task IntegrationTest_ValidOutput_WithPrintTopShortFlag()
    {
        var result = await _sut.ExecuteWithShortFlag();

        string[] lines = result.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        AssertHelper.Header(lines);

        Assert.True(lines.Length <= 3, $"Too many lines printed... Actual: {lines.Length}");
    }

    [Fact]
    public async Task IntegrationTest_InvalidOutput_WithPrintTopFlagAndInvalidValue()
    {
        var result = await _sut.ExecuteWithInvalidValue();

        Assert.Equal("fatal: Value for --print-top must be an integer\n", result);
    }

    [Fact]
    public async Task IntegrationTest_InvalidOutput_WithPrintTopFlagAndZeroValue()
    {
        var result = await _sut.ExecuteWithZeroValue();

        Assert.Equal("fatal: Value for --print-top must be greater than 0\n", result);
    }

    [Fact]
    public async Task IntegrationTest_InvalidOutput_WithPrintTopFlagAndNoValue()
    {
        var result = await _sut.ExecuteWithNoValue();

        Assert.Equal("fatal: Value for --print-top is missing\n", result);
    }

    private class TestExecutor(DefaultFixture fixture)
    {
        public async Task<string> ExecuteWithLongFlag() => await fixture.Exec("--print-top", "1");
        public async Task<string> ExecuteWithShortFlag() => await fixture.Exec("-p", "1");
        public async Task<string> ExecuteWithInvalidValue() => await fixture.Exec("--print-top", "value");
        public async Task<string> ExecuteWithZeroValue() => await fixture.Exec("--print-top", "0");
        public async Task<string> ExecuteWithNoValue() => await fixture.Exec("--print-top");
    }
}
