using Bbranch.Tests.Helpers;
using Bbranch.Tests.Infrastucture.Fixtures;

namespace Bbranch.Tests.Integration;

[Trait("Category", "Integration")]
public class NoContainsFlagTests
{
    private readonly TestExecutor _sut;

    public NoContainsFlagTests(DefaultFixture fixture)
    {
        _sut = new TestExecutor(fixture);
    }

    [Theory]
    [InlineData("main")]
    [InlineData("ma*")]
    public async Task IntegrationTest_ValidOutput_WithNoContains(string pattern)
    {
        var result = await _sut.ExecuteWithRegex(pattern);

        string[] lines = result.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        AssertHelper.Header(lines);

        foreach (string line in lines.Skip(2))
        {
            var (ahead, behind) = AheadBehindHelper.GetAheadBehind(line);

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

    [Fact]
    public async Task IntegrationTest_ValidOutput_WithNoContainsShortFlagAndMultiValue()
    {
        var result = await _sut.ExecuteWithNoContainsShortFlagAndMultiValue();

        string[] lines = result.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        AssertHelper.Header(lines);

        foreach (string line in lines.Skip(2))
        {
            var (ahead, behind) = AheadBehindHelper.GetAheadBehind(line);

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

    [Fact]
    public async Task IntegrationTest_ValidOutput_WithNoContainsLongFlagAndMultiValue()
    {
        var result = await _sut.ExecuteWithNoContainsLongFlagAndMultiValue();

        string[] lines = result.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        AssertHelper.Header(lines);

        foreach (string line in lines.Skip(2))
        {
            var (ahead, behind) = AheadBehindHelper.GetAheadBehind(line);

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

    [Fact]
    public async Task IntegrationTest_InvalidOutput_WithNoContainsAndContainsFlag()
    {
        var result = await _sut.ExecuteWithNoContainsAndContainsFlag();

        result = result.Replace("\r", "");

        Assert.Equal("fatal: Cannot use both --contains and --no-contains\n", result);
    }

    [Fact]
    public async Task IntegrationTest_InvalidOutput_WithNoContainsFlagAndNoValue()
    {
        var result = await _sut.ExecuteWithNoContainsFlagAndNoValue();

        result = result.Replace("\r", "");

        Assert.Equal("fatal: Value for --no-contains is missing\n", result);
    }

    private class TestExecutor(DefaultFixture fixture)
    {
        public async Task<string> ExecuteWithRegex(string regex) => await fixture.Exec("-n", regex);

        public async Task<string> ExecuteWithNoContainsShortFlagAndMultiValue() =>
            await fixture.Exec("-n", "main;test/branch1");

        public async Task<string> ExecuteWithNoContainsLongFlagAndMultiValue() =>
            await fixture.Exec("--no-contains", "main;test/branch1");

        public async Task<string> ExecuteWithNoContainsAndContainsFlag() =>
            await fixture.Exec("--no-contains", "main", "--contains", "main");

        public async Task<string> ExecuteWithNoContainsFlagAndNoValue() => await fixture.Exec("-n");
    }
}
