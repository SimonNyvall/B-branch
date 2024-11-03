using System.Runtime.InteropServices;

namespace Bbranch.IntegrationTests;

public class AllFlagTests
{
 [Fact]
    public async Task IntegrationTest_ValidOutput_WithAllFlag()
    {
        await IntegrationTest_ValidOutput_WithAllShortFlag();
        await IntegrationTest_ValidOutput_WithAllLongFlag();
    }

    private static async Task IntegrationTest_ValidOutput_WithAllShortFlag()
    {
        using var process = ProcessHelper.GetDotnetProcess("-a");
        process.Start();

        string output = await process.StandardOutput.ReadToEndAsync();
        string error = await process.StandardError.ReadToEndAsync();

        process.WaitForExit();

        Assert.True(string.IsNullOrEmpty(error), error);

        string[] lines = output.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        CommonTests.AssertHeader(lines);

        foreach (string line in lines.Skip(2))
        {
            var (ahead, behind) = CommonTests.GetAheadBehindFromString(line);

            Assert.True(ahead >= 0, "ahead was below 0.");
            Assert.True(behind >= 0, "behind was below 0.");
        }
    }

    private static async Task IntegrationTest_ValidOutput_WithAllLongFlag()
    {
        using var process = ProcessHelper.GetDotnetProcess("--all");
        process.Start();

        string output = await process.StandardOutput.ReadToEndAsync();
        string error = await process.StandardError.ReadToEndAsync();

        process.WaitForExit();

        Assert.True(string.IsNullOrEmpty(error), error);

        string[] lines = output.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        CommonTests.AssertHeader(lines);

        foreach (string line in lines.Skip(2))
        {
            var (ahead, behind) = CommonTests.GetAheadBehindFromString(line);

            Assert.True(ahead >= 0, "ahead was below 0.");
            Assert.True(behind >= 0, "behind was below 0.");
        }
    }
}