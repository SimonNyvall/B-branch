namespace Bbranch.IntegrationTests;

public class SortFlagTests : IntegrationBase
{
    [Fact(Timeout = 120000)]
    public async Task IntegrationTest_ValidOutput_WithSortFlag()
    {
        await IntegrationTest_ValidOutput_WithSortShortFlag();
        await IntegrationTest_ValidOutput_WithSortLongFlag();
    }

    private async Task IntegrationTest_ValidOutput_WithSortShortFlag()
    {
        using var process = GetDotnetProcess("-s", "name");

        var (output, error) = await RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        string[] lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        AssertHeader(lines);

        foreach (string line in lines.Skip(2))
        {
            var (ahead, behind) = GetAheadBehindFromString(line);

            Assert.True(ahead >= 0, $"ahead was below 0... Actual: {ahead}... Line: {line}");
            Assert.True(behind >= 0, $"behind was below 0... Actual: {behind} Line: {line}");
        }

        string[] branchNames = lines.Skip(2).Select(l => l.Split('|')[2].Trim()).ToArray();

        string[] sortedBranchNames = branchNames.OrderBy(b => b).ToArray();

        Assert.Equal(branchNames, sortedBranchNames);
    }

    private async Task IntegrationTest_ValidOutput_WithSortLongFlag()
    {
        using var process = GetDotnetProcess("--sort", "name");

        var (output, error) = await RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        string[] lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        AssertHeader(lines);

        foreach (string line in lines.Skip(2))
        {
            var (ahead, behind) = GetAheadBehindFromString(line);

            Assert.True(ahead >= 0, $"ahead was below 0... Actual: {ahead}... Line: {line}");
            Assert.True(behind >= 0, $"behind was below 0... Actual: {behind} Line: {line}");
        }

        string[] branchNames = lines.Skip(2).Select(l => l.Split('|')[2].Trim()).ToArray();

        string[] sortedBranchNames = branchNames.OrderBy(b => b).ToArray();

        Assert.Equal(branchNames, sortedBranchNames);
    }
}