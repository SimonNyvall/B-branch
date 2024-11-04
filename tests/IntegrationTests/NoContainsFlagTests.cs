namespace Bbranch.IntegrationTests;

[Collection("Sequential")]
public class NoContainsFlagTests : IntegrationBase
{
    [Fact(Timeout = 120000)]
    public async Task IntegrationTest_ValidOutput_WithNoContainsShortFlag()
    {
        using var process = GetBbranchProcessWithoutPager("-n", "main");

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

        Assert.All(branchNames, b => Assert.DoesNotContain("main", b, StringComparison.OrdinalIgnoreCase));
    }

    [Fact(Timeout = 120000)]
    public async Task IntegrationTest_ValidOutput_WithNoContainsLongFlag()
    {
        using var process = GetBbranchProcessWithoutPager("--no-contains", "main");

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

        Assert.All(branchNames, b => Assert.DoesNotContain("main", b, StringComparison.OrdinalIgnoreCase));
    }
}