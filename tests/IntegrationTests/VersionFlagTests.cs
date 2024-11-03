using System.Text.RegularExpressions;

namespace Bbranch.IntegrationTests;

public class VersionFlagTests : IntegrationBase
{
    [Fact(Timeout = 120000)]
    public async Task IntegrationTest_ValidOutput_WithVersionFlag()
    {
        await IntegrationTest_ValidOutput_WithVersionShortFlag();
        await IntegrationTest_ValidOutput_WithVersionLongFlag();
    }

    private async Task IntegrationTest_ValidOutput_WithVersionShortFlag()
    {
        using var process = GetDotnetProcess(true, "-v");
       
        var (output, error) = await RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        const string pattern = @"v(\d+)\.(\d+)\.(\d+)";

        Match match = Regex.Match(output, pattern);

        Assert.True(match.Success, "Failed to match version pattern.");
    }

    private async Task IntegrationTest_ValidOutput_WithVersionLongFlag()
    {
        using var process = GetDotnetProcess(true, "-v");

        var (output, error) = await RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        const string pattern = @"v(\d+)\.(\d+)\.(\d+)";

        Match match = Regex.Match(output, pattern);

        Assert.True(match.Success, "Failed to match version pattern.");
    }
}