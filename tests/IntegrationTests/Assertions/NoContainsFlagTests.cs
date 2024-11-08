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

        Assert.Equal(3, branchNames.Length);
        Assert.DoesNotContain("main", branchNames);
        Assert.Contains("test/branch1", branchNames);
        Assert.Contains("test/branch2", branchNames);
        Assert.Contains("test/branch3", branchNames);
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

        Assert.Equal(3, branchNames.Length);
        Assert.DoesNotContain("main", branchNames);
        Assert.Contains("test/branch1", branchNames);
        Assert.Contains("test/branch2", branchNames);
        Assert.Contains("test/branch3", branchNames);
    }

    [Fact(Timeout = 120000)]
    public async Task IntegrationTest_ValidOutput_WithNoContainsShortFlagAndMultiValue()
    {
        using var process = GetBbranchProcessWithoutPager("-n", "main;test/branch1");

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

        Assert.Equal(2, branchNames.Length);
        Assert.DoesNotContain("main", branchNames);
        Assert.DoesNotContain("test/branch1", branchNames);
        Assert.Contains("test/branch2", branchNames);
        Assert.Contains("test/branch3", branchNames);
    }

    [Fact(Timeout = 120000)]
    public async Task IntegrationTest_ValidOutput_WithNoContainsLongFlagAndMultiValue()
    {
        using var process = GetBbranchProcessWithoutPager("--no-contains", "main;test/branch1");

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

        Assert.Equal(2, branchNames.Length);
        Assert.DoesNotContain("main", branchNames);
        Assert.DoesNotContain("test/branch1", branchNames);
        Assert.Contains("test/branch2", branchNames);
        Assert.Contains("test/branch3", branchNames);
    }

    [Fact(Timeout = 120000)]
    public async Task IntegrationTest_InvalidOutput_WithNoContainsAndContainsFlag()
    {
        using var process = GetBbranchProcessWithoutPager("--no-contains", "main", "--contains", "main");

        var (output, error) = await RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        output = output.Replace("\r", "");

        Assert.Equal("fatal: Cannot use both --contains and --no-contains\n", output);
    }

    [Fact(Timeout = 120000)]
    public async Task IntegrationTest_InvalidOutput_WithNoContainsFlagAndNoValue()
    {
        using var process = GetBbranchProcessWithoutPager("-n");

        var (output, error) = await RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        output = output.Replace("\r", "");

        Assert.Equal("fatal: Value for --no-contains is missing\n", output);
    }
}