namespace Bbranch.IntegrationTests;

public class ContainsFlagTests : IntegrationBase
{
    [Fact]
    public async Task IntegrationTest_ValidOutput_WithContainsFlag()
    {
        await IntegrationTest_ValidOutput_WithContainsShortFlag();
        await IntegrationTest_ValidOutput_WithContainsLongFlag();
    }

    private static async Task IntegrationTest_ValidOutput_WithContainsShortFlag()
    {
        using var process = GetDotnetProcess("-c", "main");
        process.Start();

        string output = await process.StandardOutput.ReadToEndAsync();
        string error = await process.StandardError.ReadToEndAsync();

        process.WaitForExit();

        Assert.True(string.IsNullOrEmpty(error), error);

        string[] lines = output.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        AssertHeader(lines);

        foreach (string line in lines.Skip(2))
        {
            var (ahead, behind) = GetAheadBehindFromString(line);

            Assert.True(ahead >= 0, "ahead was below 0.");
            Assert.True(behind >= 0, "behind was below 0.");
        }

        string[] branchNames = lines.Skip(2).Select(l => l.Split('|')[2].Trim()).ToArray();

        Assert.All(branchNames, b => Assert.Contains("main", b, StringComparison.OrdinalIgnoreCase));
    }

    private static async Task IntegrationTest_ValidOutput_WithContainsLongFlag()
    {
        using var process = GetDotnetProcess("--contains", "main");
        process.Start();

        string output = await process.StandardOutput.ReadToEndAsync();
        string error = await process.StandardError.ReadToEndAsync();

        process.WaitForExit();

        Assert.True(string.IsNullOrEmpty(error), error);

        string[] lines = output.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        AssertHeader(lines);

        foreach (string line in lines.Skip(2))
        {
            var (ahead, behind) = GetAheadBehindFromString(line);

            Assert.True(ahead >= 0, "ahead was below 0.");
            Assert.True(behind >= 0, "behind was below 0.");
        }

        string[] branchNames = lines.Skip(2).Select(l => l.Split('|')[2].Trim()).ToArray();

        Assert.All(branchNames, b => Assert.Contains("main", b, StringComparison.OrdinalIgnoreCase));
    }
}