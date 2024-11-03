namespace Bbranch.IntegrationTests;

public class RemoteFlagTests : IntegrationBase
{
    [Fact]
    public async Task IntegrationTest_ValidOutput_WithRemoteFlag()
    {
        await IntegrationTest_ValidOutput_WithRemoteShortFlag();
        await IntegrationTest_ValidOutput_WithRemoteLongFlag();
    }

    private static async Task IntegrationTest_ValidOutput_WithRemoteShortFlag()
    {
        using var process = GetDotnetProcess("-r");
        process.Start();

        string output = await process.StandardOutput.ReadToEndAsync();
        string error = await process.StandardError.ReadToEndAsync();

        process.WaitForExit();

        Assert.True(string.IsNullOrEmpty(error), error);

        string[] lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        AssertHeader(lines);

        foreach (string line in lines.Skip(2))
        {
            var (ahead, behind) = GetAheadBehindFromString(line);

            Assert.True(ahead >= 0, "ahead was below 0.");
            Assert.True(behind >= 0, "behind was below 0.");
        }
    }

    private static async Task IntegrationTest_ValidOutput_WithRemoteLongFlag()
    {
        using var process = GetDotnetProcess("--remote");
        process.Start();

        string output = await process.StandardOutput.ReadToEndAsync();
        string error = await process.StandardError.ReadToEndAsync();

        process.WaitForExit();

        Assert.True(string.IsNullOrEmpty(error), error);

        string[] lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        AssertHeader(lines);

        foreach (string line in lines.Skip(2))
        {
            var (ahead, behind) = GetAheadBehindFromString(line);

            Assert.True(ahead >= 0, "ahead was below 0.");
            Assert.True(behind >= 0, "behind was below 0.");
        }
    }
}