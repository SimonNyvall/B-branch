using System.Text.RegularExpressions;

namespace Bbranch.IntegrationTests;

[Collection("Sequential")]
public class VersionFlagTests : IntegrationBase
{
    [Fact(Timeout = 120000)]
    public async Task IntegrationTest_ValidOutput_WithVersionShortFlag()
    {
        using var process = GetBbranchProcess("-v");

        var (output, error) = await RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        const string pattern = @"v(\d+)\.(\d+)\.(\d+)";

        Match match = Regex.Match(output, pattern);

        Assert.True(match.Success, "Failed to match version pattern.");
    }

    [Fact(Timeout = 120000)]
    public async Task IntegrationTest_ValidOutput_WithVersionLongFlag()
    {
        using var process = GetBbranchProcess("-v");

        var (output, error) = await RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        const string pattern = @"v(\d+)\.(\d+)\.(\d+)";

        Match match = Regex.Match(output, pattern);

        Assert.True(match.Success, "Failed to match version pattern.");
    }

    [Fact(Timeout = 120000)]
    public async Task IntegrationTest_InvalidOutput_WithVersionFlagAndValue()
    {
        using var process = GetBbranchProcess("--version", "value");

        var (output, error) = await RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        output = output.Replace("\r", "");

        Assert.Equal("fatal: Value for --version is not allowed\n", output);
    }

    [Fact(Timeout = 120000)]
    public async Task IntegrationTest_InvalidOutput_WithVersionFlagAndOtherFlag()
    {
        using var process = GetBbranchProcess("--version", "-a");

        var (output, error) = await RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        output = output.Replace("\r", "");

        Assert.Equal("fatal: --version cannot be used with any other option\n", output);
    }
}