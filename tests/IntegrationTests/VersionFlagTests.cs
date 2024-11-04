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
}