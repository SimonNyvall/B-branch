using Bbranch.Tests.Helpers;
using Bbranch.Tests.Infrastucture.Fixtures;

namespace Bbranch.Tests.Integration;

[Trait("Category", "Integration")]
public class AllFlagTests
{
    private readonly TestExecutor _sut;

    public AllFlagTests(DefaultFixture fixture)
    {
        _sut = new TestExecutor(fixture);
    }

    [Fact]
    public async Task IntegrationTest_ValidOutput_WithAllShortFlag()
    {
        var result = await _sut.ExecuteWithShortFlag();

        string[] lines = result.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        AssertHelper.Header(lines);

        foreach (string line in lines.Skip(2))
        {
            var (ahead, behind) = AheadBehindHelper.GetAheadBehind(line);
            Assert.True(ahead >= 0, $"ahead was below 0... Actual: {ahead}... Line: {line}");
            Assert.True(behind >= 0, $"behind was below 0... Actual: {behind} Line: {line}");
        }
    }

    [Fact]
    public async Task IntegrationTest_ValidOutput_WithAllLongFlag()
    {
        var result = await _sut.ExecuteWithLongFlag();

        string[] lines = result.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        AssertHelper.Header(lines);

        foreach (string line in lines.Skip(2))
        {
            var (ahead, behind) = AheadBehindHelper.GetAheadBehind(line);

            Assert.True(ahead >= 0, $"ahead was below 0... Actual: {ahead}... Line: {line}");
            Assert.True(behind >= 0, $"behind was below 0... Actual: {behind} Line: {line}");
        }
    }

    [Fact]
    public async Task IntegrationTest_InvalidOutput_WithAllFlagAndValue()
    {
        var result = await _sut.ExecuteWithLongFlagAndValue();

        Assert.Equal("fatal: Value for --all is not allowed\n", result);
    }

    [Fact]
    public async Task IntegrationTest_InvalidOutput_WithAllAndRemoteFlag()
    {
        var result = await _sut.ExecuteWithLongFlagAndRemoteFlag();

        Assert.Equal("fatal: Cannot use both --all and --remote\n", result);
    }

    private class TestExecutor(DefaultFixture fixture)
    {
        public async Task<string>  ExecuteWithShortFlag() => await fixture.Exec("-a");
        public async Task<string>  ExecuteWithLongFlag() => await fixture.Exec("--all");
        public async Task<string>  ExecuteWithLongFlagAndValue() => await fixture.Exec("--all", "value");
        public async Task<string>  ExecuteWithLongFlagAndRemoteFlag() => await fixture.Exec("--all", "--remote");
    }
}
