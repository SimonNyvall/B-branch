namespace Bbranch.IntegrationTests;

public class SortFlagTests
{
    [Fact]
    public async Task IntegrationTest_ValidOutput_WithSortFlag()
    {
        await IntegrationTest_ValidOutput_WithSortShortFlag();
        await IntegrationTest_ValidOutput_WithSortLongFlag();
    }

    private static async Task IntegrationTest_ValidOutput_WithSortShortFlag()
    {
        using var process = ProcessHelper.GetDotnetProcess("-s", "name");
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

        string[] sortedBranchNames = branchNames.OrderBy(b => b).ToArray();

        Assert.Equal(branchNames, sortedBranchNames);
    }

    private static async Task IntegrationTest_ValidOutput_WithSortLongFlag()
    {
        using var process = ProcessHelper.GetDotnetProcess("--sort", "name");
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

        string[] sortedBranchNames = branchNames.OrderBy(b => b).ToArray();

        Assert.Equal(branchNames, sortedBranchNames);
    }
}