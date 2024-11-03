namespace Bbranch.IntegrationTests;

public class TrackFlagTests
{
    [Fact]
    public async Task IntegrationTest_ValidOutput_WithTrackFlag()
    {
        await IntegrationTest_ValidOutput_WithTrackShortFlag();
        await IntegrationTest_ValidOutput_WithTrackLongFlag();
    }

    private static async Task IntegrationTest_ValidOutput_WithTrackShortFlag()
    {
        using var process = ProcessHelper.GetDotnetProcess("-t", "main");
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

    private static async Task IntegrationTest_ValidOutput_WithTrackLongFlag()
    {
        using var process = ProcessHelper.GetDotnetProcess("--track", "main");
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