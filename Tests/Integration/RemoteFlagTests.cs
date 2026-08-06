using Bbranch.Tests.Helpers;
using Bbranch.Tests.Infrastucture.Fixtures;

namespace Bbranch.Tests.Integration;

[Trait("Category", "Integration")]
public class RemoteFlagTests
{
    private readonly TestExecutor _sut;

    public RemoteFlagTests(DefaultFixture fixture)
    {
        _sut = new TestExecutor(fixture);
    }

    [Fact]
    public async Task IntegrationTest_ValidOutput_WithRemoteShortFlag()
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
    public async Task IntegrationTest_ValidOutput_WithRemoteLongFlag()
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
    public async Task IntegrationTest_InvalidOutput_WithRemoteFlagAndValue()
    {
        var result = await _sut.ExecuteWithValue();

        Assert.Equal("fatal: Value for --remote is not allowed\n", result);
    }

    [Fact]
    public async Task IntegrationTest_InvalidOutput_WithRemoteAndAllFlag()
    {
        var result = await _sut.ExecuteWithAllFlag();

        Assert.Equal("fatal: Cannot use both --all and --remote\n", result);
    }

    private class TestExecutor(DefaultFixture fixture)
    {
        public async Task<string> ExecuteWithShortFlag() => await fixture.Exec("-r");
        public async Task<string> ExecuteWithLongFlag() => await fixture.Exec("--remote");
        public async Task<string> ExecuteWithValue() => await fixture.Exec("--remote", "value");
        public async Task<string> ExecuteWithAllFlag() => await fixture.Exec("--remote", "--all");
    }
}
