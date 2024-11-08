using System.Diagnostics;
using Xunit.Abstractions;

namespace Bbranch.IntegrationTests;

[Collection("Sequential")]
public class AllFlagTests : IntegrationBase
{
    private readonly ITestOutputHelper _output;

    public AllFlagTests(ITestOutputHelper output)
    {
        _output = output;
        WarmUp();
    }

    [Fact(Timeout = 120000)]
    public async Task IntegrationTest_ValidOutput_WithAllShortFlag()
    {
        using var process = GetBbranchProcessWithoutPager("-a");

        var (output, error) = await RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        string[] lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        AssertHeader(lines);

        foreach (string line in lines.Skip(2))
        {
            var (ahead, behind) = GetAheadBehindFromString(line);
            Assert.True(ahead >= 0, $"ahead was below 0... Actual: {ahead}... Line: {line}");
            Assert.True(behind >= 0, $"behind was below 0... Actual: {behind} Line: {line}");
        }
    }

    [Fact(Timeout = 120000)]
    public async Task IntegrationTest_ValidOutput_WithAllLongFlag()
    {
        using var process = GetBbranchProcessWithoutPager("--all");

        var (output, error) = await RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        string[] lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        AssertHeader(lines);

        foreach (string line in lines.Skip(2))
        {
            var (ahead, behind) = GetAheadBehindFromString(line);

            Assert.True(ahead >= 0, $"ahead was below 0... Actual: {ahead}... Line: {line}");
            Assert.True(behind >= 0, $"behind was below 0... Actual: {behind} Line: {line}");
        }
    }

    [Fact(Timeout = 120000)]
    public async Task IntegrationTest_InvalidOutput_WithAllFlagAndValue()
    {
        using var process = GetBbranchProcessWithoutPager("--all", "value");

        var (output, error) = await RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        output = output.Replace("\r", "");

        Assert.Equal("fatal: Value for --all is not allowed\n", output);
    }

    [Fact(Timeout = 120000)]
    public async Task IntegrationTest_InvalidOutput_WithAllAndRemoteFlag()
    {
        using var process = GetBbranchProcessWithoutPager("--all", "--remote");

        var (output, error) = await RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        output = output.Replace("\r", "");

        Assert.Equal("fatal: Cannot use both --all and --remote\n", output);
    }
}