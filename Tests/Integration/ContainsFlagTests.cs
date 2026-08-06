using Bbranch.Tests.Helpers;
using Bbranch.Tests.Infrastucture.Fixtures;

namespace Bbranch.Tests.Integration;

[Trait("Category", "Integration")]
public class ContainsFlagTests
{
    private readonly TestExecutor _sut;

    public ContainsFlagTests(DefaultFixture fixture)
    {
        _sut = new TestExecutor(fixture);
    }

    [Theory]
    [InlineData("main")]
    [InlineData("ma*")]
    public async Task IntegrationTest_ValidOutput_WithContains(string pattern)
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

        Assert.Contains("main", branchNames);
        Assert.DoesNotContain("test/branch1", branchNames);
        Assert.DoesNotContain("test/branch2", branchNames);
        Assert.DoesNotContain("test/branch3", branchNames);
    }

    [Fact]
    public async Task IntegrationTest_ValidOutput_WithContainsShortFlagAndMultiValue()
    {
        var result = await _sut.ExecuteWithLongFlagAndMultiValue();

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
        Assert.Contains("main", branchNames);
        Assert.Contains("test/branch1", branchNames);
        Assert.DoesNotContain("test/branch2", branchNames);
        Assert.DoesNotContain("test/branch3", branchNames);
    }

    [Fact]
    public async Task IntegrationTest_ValidOutput_WithContainsLongFlagAndMultiValue()
    {
        var result = await _sut.ExecuteWithLongFlagAndMultiValue();

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
        Assert.Contains("main", branchNames);
        Assert.Contains("test/branch1", branchNames);
        Assert.DoesNotContain("test/branch2", branchNames);
        Assert.DoesNotContain("test/branch3", branchNames);
    }

    [Fact]
    public async Task IntegrationTest_InvalidOutput_WithContainsAndNoContainsFlag()
    {
        var result = await _sut.ExecuteWithContainsAndNoContainsFlag();

        Assert.Equal("fatal: Cannot use both --contains and --no-contains\n", result);
    }

    [Fact]
    public async Task IntegrationTest_InvalidOutput_WithContainsFlagAndNoValue()
    {
        var result = await _sut.ExecuteWithNoValue();

        Assert.Equal("fatal: Value for --contains is missing\n", result);
    }

    private class TestExecutor(DefaultFixture fixture)
    {
        public async Task<string> ExecuteWithRegex(string regex) => await fixture.Exec("-c", regex);
        public async Task<string> ExecuteWithShortFlagAndMultiValue() => await fixture.Exec("-c", "main;test/branch1");
        public async Task<string> ExecuteWithLongFlagAndMultiValue() => await fixture.Exec("--contains", "main;test/branch1");
        public async Task<string> ExecuteWithContainsAndNoContainsFlag() => await fixture.Exec("");
        public async Task<string> ExecuteWithNoValue() => await fixture.Exec("-c");
    }
}
