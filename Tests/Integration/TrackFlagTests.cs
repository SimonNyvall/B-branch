using Bbranch.Tests.Helpers;
using Bbranch.Tests.Infrastucture.Fixtures;

namespace Bbranch.Tests.Integration;

[Trait("Category", "Integration")]
public class TrackFlagTests
{
    private readonly TestExecutor _sut;

    public TrackFlagTests(DefaultFixture fixture)
    {
        _sut = new TestExecutor(fixture);
    }

    [Fact]
    public async Task IntegrationTest_ValidOutput_WithTrackShortFlag()
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
    public async Task IntegrationTest_ValidOutput_WithTrackLongFlag()
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
    public async Task IntegrationTest_InvalidOutput_WithTrackFlagAndNoValue()
    {
        var result = await _sut.ExecuteWithNoValue();

        Assert.Equal("fatal: Value for --track is missing\n", result);
    }

    private class TestExecutor(DefaultFixture fixture)
    {
        public async Task<string> ExecuteWithShortFlag() => await fixture.Exec("-t", "main");

        public async Task<string> ExecuteWithLongFlag() => await fixture.Exec("--track", "main");

        public async Task<string> ExecuteWithNoValue() => await fixture.Exec("--track");
    }
}
