namespace Bbranch.IntegrationTests;

[Collection(Constants.DefaultFixtureName)]
[Trait("Category", "Integration")]
public class TrackFlagTests
{
    private readonly DefaultFixture _fixture;

    public TrackFlagTests(DefaultFixture fixture)
    {
        _fixture = fixture;
    }

    [IntegrationFact]
    public async Task IntegrationTest_ValidOutput_WithTrackShortFlag()
    {
        using var process = _fixture.GetBbranchProcess("-t", "main");

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
    }

    [IntegrationFact]
    public async Task IntegrationTest_ValidOutput_WithTrackLongFlag()
    {
        using var process = _fixture.GetBbranchProcess("--track", "main");

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
    }

    [IntegrationFact]
    public async Task IntegrationTest_InvalidOutput_WithTrackFlagAndNoValue()
    {
        using var process = _fixture.GetBbranchProcess("--track");

        var (output, error) = await _fixture.RunProcessWithTimeoutAsync(process);

        output = output.Replace("\r", "");

        Assert.True(string.IsNullOrEmpty(error), error);
        Assert.Equal("fatal: Value for --track is missing\n", output);
    }
}
