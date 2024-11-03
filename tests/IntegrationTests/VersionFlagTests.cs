using System.Text.RegularExpressions;

namespace Bbranch.IntegrationTests;

public class VersionFlagTests
{
    [Fact]
    public async Task IntegrationTest_ValidOutput_WithVersionFlag()
    {
        await IntegrationTest_ValidOutput_WithVersionShortFlag();
        await IntegrationTest_ValidOutput_WithVersionLongFlag();
    }

    private static async Task IntegrationTest_ValidOutput_WithVersionShortFlag()
    {
        using var process = ProcessHelper.GetDotnetProcess("-v");
        process.Start();

        string output = await process.StandardOutput.ReadToEndAsync();
        string error = await process.StandardError.ReadToEndAsync();

        Assert.True(string.IsNullOrEmpty(error), error);

        const string pattern = @"v(\d+)\.(\d+)\.(\d+)";

        Match match = Regex.Match(output, pattern);

        Assert.True(match.Success, "Failed to match version pattern.");
    }

    private static async Task IntegrationTest_ValidOutput_WithVersionLongFlag()
    {
        using var process = ProcessHelper.GetDotnetProcess("-v");
        process.Start();

        string output = await process.StandardOutput.ReadToEndAsync();
        string error = await process.StandardError.ReadToEndAsync();

        Assert.True(string.IsNullOrEmpty(error), error);

        const string pattern = @"v(\d+)\.(\d+)\.(\d+)";

        Match match = Regex.Match(output, pattern);

        Assert.True(match.Success, "Failed to match version pattern.");
    }
}