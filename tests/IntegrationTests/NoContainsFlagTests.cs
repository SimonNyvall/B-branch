namespace Bbranch.IntegrationTests;

public class NoContainsFlagTests
{
    [Fact]
    public async Task IntegrationTest_ValidOutput_WithNoContainsFlag()
    {
        await IntegrationTest_ValidOutput_WithNoContainsShortFlag();
        await IntegrationTest_ValidOutput_WithNoContainsLongFlag();
    }

    private static async Task IntegrationTest_ValidOutput_WithNoContainsShortFlag()
    {
        using var process = ProcessHelper.GetDotnetProcess("-n", "main");
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

        string[] branchNames = lines.Skip(2).Select(l => l.Split('|')[2].Trim()).ToArray();

        Assert.All(branchNames, b => Assert.DoesNotContain("main", b, StringComparison.OrdinalIgnoreCase));
    }

    private static async Task IntegrationTest_ValidOutput_WithNoContainsLongFlag()
    {
        using var process = ProcessHelper.GetDotnetProcess("--no-contains", "main");
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

        string[] branchNames = lines.Skip(2).Select(l => l.Split('|')[2].Trim()).ToArray();

        Assert.All(branchNames, b => Assert.DoesNotContain("main", b, StringComparison.OrdinalIgnoreCase));
    }
}