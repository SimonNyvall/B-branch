namespace Bbranch.IntegrationTests;

[Collection(Constants.DefaultFixtureName)]
[Trait("Category", "Integration")]
public class ContainsFlagTests
{
    private readonly DefaultFixture _fixture;

    public ContainsFlagTests(DefaultFixture fixture)
    {
        _fixture = fixture;
    }

    [IntegrationTheory]
    [InlineData("-c", "main")]
    [InlineData("-c", "ma*")]
    [InlineData("--contains", "main")]
    [InlineData("--contains", "ma*")]
    public async Task IntegrationTest_ValidOutput_WithContains(string command, string pattern)
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

        string[] branchNames = lines.Skip(2).Select(l => l.Split('|')[2].Trim()).ToArray();

        Assert.Contains("main", branchNames);
        Assert.DoesNotContain("test/branch1", branchNames);
        Assert.DoesNotContain("test/branch2", branchNames);
        Assert.DoesNotContain("test/branch3", branchNames);
    }

    [IntegrationFact]
    public async Task IntegrationTest_ValidOutput_WithContainsShortFlagAndMultiValue()
    {
        using var process = _fixture.GetBbranchProcess("--contains", "main;test/branch1");

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

        string[] branchNames = lines.Skip(2).Select(l => l.Split('|')[2].Trim()).ToArray();

        Assert.Equal(2, branchNames.Length);
        Assert.Contains("main", branchNames);
        Assert.Contains("test/branch1", branchNames);
        Assert.DoesNotContain("test/branch2", branchNames);
        Assert.DoesNotContain("test/branch3", branchNames);
    }

    [IntegrationFact]
    public async Task IntegrationTest_ValidOutput_WithContainsLongFlagAndMultiValue()
    {
        using var process = _fixture.GetBbranchProcess("-c", "main;test/branch1");

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

        string[] branchNames = lines.Skip(2).Select(l => l.Split('|')[2].Trim()).ToArray();

        Assert.Equal(2, branchNames.Length);
        Assert.Contains("main", branchNames);
        Assert.Contains("test/branch1", branchNames);
        Assert.DoesNotContain("test/branch2", branchNames);
        Assert.DoesNotContain("test/branch3", branchNames);
    }

    [IntegrationFact]
    public async Task IntegrationTest_InvalidOutput_WithContainsAndNoContainsFlag()
    {
        using var process = _fixture.GetBbranchProcess("-c", "main", "-n", "main");

        var (output, error) = await _fixture.RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        output = output.Replace("\r", "");

        Assert.Equal("fatal: Cannot use both --contains and --no-contains\n", output);
    }

    [IntegrationFact]
    public async Task IntegrationTest_InvalidOutput_WithContainsFlagAndNoValue()
    {
        using var process = _fixture.GetBbranchProcess("--contains");

        var (output, error) = await _fixture.RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        output = output.Replace("\r", "");

        Assert.Equal("fatal: Value for --contains is missing\n", output);
    }

    [IntegrationFact]
    public async Task IntegrationTest_ValidOutput_WithContainsFlagAndRegex()
    {
        using var process = _fixture.GetBbranchProcess("-c", "ma*");

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

        string[] branchNames = lines.Skip(2).Select(l => l.Split('|')[2].Trim()).ToArray();

        Assert.Contains("main", branchNames);
        Assert.DoesNotContain("test/branch1", branchNames);
        Assert.DoesNotContain("test/branch2", branchNames);
        Assert.DoesNotContain("test/branch3", branchNames);
    }
}
