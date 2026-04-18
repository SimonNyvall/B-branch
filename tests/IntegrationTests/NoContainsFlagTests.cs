namespace Bbranch.IntegrationTests;

[Collection(Constants.DefaultFixtureName)]
public class NoContainsFlagTests
{
    private readonly DefaultFixture _fixture;

    public NoContainsFlagTests(DefaultFixture fixture)
    {
        _fixture = fixture;
    }

    [Theory]
    [InlineData("-n", "main")]
    [InlineData("-n", "ma*")]
    [InlineData("--no-contains", "main")]
    [InlineData("--no-contains", "ma*")]
    public async Task IntegrationTest_ValidOutput_WithNoContains(string command, string pattern)
    {
        using var process = _fixture.GetBbranchProcess(command, pattern);

        var (output, error) = await _fixture.RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        string[] lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        _fixture.AssertHeader(lines);

        foreach (string line in lines.Skip(2))
        {
            var (ahead, behind) = _fixture.GetAheadBehindFromString(line);

            Assert.True(ahead >= 0, $"ahead was below 0... Actual: {ahead}... Line: {line}");
            Assert.True(behind >= 0, $"behind was below 0... Actual: {behind} Line: {line}");
        }

        string[] branchNames = lines
            .Skip(2)
            .Select(l => l.Split('|')[2].Trim())
            .Select(l => _fixture.RemoveUnixChars(l).Trim())
            .ToArray();

        Assert.Equal(3, branchNames.Length);
        Assert.DoesNotContain("main", branchNames);
        Assert.Contains("test/branch1", branchNames);
        Assert.Contains("test/branch2", branchNames);
        Assert.Contains("test/branch3", branchNames);
    }

    [Fact]
    public async Task IntegrationTest_ValidOutput_WithNoContainsShortFlagAndMultiValue()
    {
        using var process = _fixture.GetBbranchProcess("-n", "main;test/branch1");

        var (output, error) = await _fixture.RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        string[] lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        _fixture.AssertHeader(lines);

        foreach (string line in lines.Skip(2))
        {
            var (ahead, behind) = _fixture.GetAheadBehindFromString(line);

            Assert.True(ahead >= 0, $"ahead was below 0... Actual: {ahead}... Line: {line}");
            Assert.True(behind >= 0, $"behind was below 0... Actual: {behind} Line: {line}");
        }

        string[] branchNames = lines
            .Skip(2)
            .Select(l => l.Split('|')[2].Trim())
            .Select(l => _fixture.RemoveUnixChars(l).Trim())
            .ToArray();

        Assert.Equal(2, branchNames.Length);
        Assert.DoesNotContain("main", branchNames);
        Assert.DoesNotContain("test/branch1", branchNames);
        Assert.Contains("test/branch2", branchNames);
        Assert.Contains("test/branch3", branchNames);
    }

    [Fact]
    public async Task IntegrationTest_ValidOutput_WithNoContainsLongFlagAndMultiValue()
    {
        using var process = _fixture.GetBbranchProcess("--no-contains", "main;test/branch1");

        var (output, error) = await _fixture.RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        string[] lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        _fixture.AssertHeader(lines);

        foreach (string line in lines.Skip(2))
        {
            var (ahead, behind) = _fixture.GetAheadBehindFromString(line);

            Assert.True(ahead >= 0, $"ahead was below 0... Actual: {ahead}... Line: {line}");
            Assert.True(behind >= 0, $"behind was below 0... Actual: {behind} Line: {line}");
        }

        string[] branchNames = lines
            .Skip(2)
            .Select(l => l.Split('|')[2].Trim())
            .Select(l => _fixture.RemoveUnixChars(l).Trim())
            .ToArray();

        Assert.Equal(2, branchNames.Length);
        Assert.DoesNotContain("main", branchNames);
        Assert.DoesNotContain("test/branch1", branchNames);
        Assert.Contains("test/branch2", branchNames);
        Assert.Contains("test/branch3", branchNames);
    }

    [Fact]
    public async Task IntegrationTest_InvalidOutput_WithNoContainsAndContainsFlag()
    {
        using var process = _fixture.GetBbranchProcess("--no-contains", "main", "--contains", "main");

        var (output, error) = await _fixture.RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        output = output.Replace("\r", "");

        Assert.Equal("fatal: Cannot use both --contains and --no-contains\n", output);
    }

    [Fact]
    public async Task IntegrationTest_InvalidOutput_WithNoContainsFlagAndNoValue()
    {
        using var process = _fixture.GetBbranchProcess("-n");

        var (output, error) = await _fixture.RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        output = output.Replace("\r", "");

        Assert.Equal("fatal: Value for --no-contains is missing\n", output);
    }
}
