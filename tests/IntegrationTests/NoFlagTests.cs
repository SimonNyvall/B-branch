namespace Bbranch.IntegrationTests;

public class NoFlagTests
{
    [Fact]
    public async Task IntegrationTest_ValidOutput_WithNoFlags()
    {
        using var process = ProcessHelper.GetDotnetProcess();
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

