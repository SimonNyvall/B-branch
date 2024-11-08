namespace Bbranch.IntegrationTests;

[Collection("Sequential")]
public class ContainsFlagTests : IntegrationBase
{
    [Fact(Timeout = 120000)]
    public async Task IntegrationTest_ValidOutput_WithContainsShortFlag()
    {
        using var process = GetBbranchProcessWithoutPager("-c", "main");

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

        Assert.Contains("main", branchNames);
        Assert.DoesNotContain("test/branch1", branchNames);
        Assert.DoesNotContain("test/branch2", branchNames);
        Assert.DoesNotContain("test/branch3", branchNames);
    }

    [Fact(Timeout = 120000)]
    public async Task IntegrationTest_ValidOutput_WithContainsLongFlag()
    {
        using var process = GetBbranchProcessWithoutPager("--contains", "main");

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

        Assert.Contains("main", branchNames);
        Assert.DoesNotContain("test/branch1", branchNames);
        Assert.DoesNotContain("test/branch2", branchNames);
        Assert.DoesNotContain("test/branch3", branchNames);
    }

    [Fact(Timeout = 120000)]
    public async Task IntegrationTest_ValidOutput_WithContainsShortFlagAndMultiValue()
    {
        using var process = GetBbranchProcessWithoutPager("--contains", "main;test/branch1");

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
        Assert.Contains("main", branchNames);
        Assert.Contains("test/branch1", branchNames);
        Assert.DoesNotContain("test/branch2", branchNames);
        Assert.DoesNotContain("test/branch3", branchNames);
    }

    [Fact(Timeout = 120000)]
    public async Task IntegrationTest_ValidOutput_WithContainsLongFlagAndMultiValue()
    {
        using var process = GetBbranchProcessWithoutPager("-c", "main;test/branch1");

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
        Assert.Contains("main", branchNames);
        Assert.Contains("test/branch1", branchNames);
        Assert.DoesNotContain("test/branch2", branchNames);
        Assert.DoesNotContain("test/branch3", branchNames);
    }

    [Fact(Timeout = 120000)]
    public async Task IntegrationTest_InvalidOutput_WithContainsAndNoContainsFlag()
    {
        using var process = GetBbranchProcessWithoutPager("-c", "main", "-n", "main");

        var (output, error) = await RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        output = output.Replace("\r", "");

        Assert.Equal("fatal: Cannot use both --contains and --no-contains\n", output);
    }

    [Fact(Timeout = 120000)]
    public async Task IntegrationTest_InvalidOutput_WithContainsFlagAndNoValue()
    {
        using var process = GetBbranchProcessWithoutPager("--contains");

        var (output, error) = await RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        output = output.Replace("\r", "");

        Assert.Equal("fatal: Value for --contains is missing\n", output);
    }
}