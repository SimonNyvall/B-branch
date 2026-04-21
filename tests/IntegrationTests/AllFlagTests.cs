namespace Bbranch.IntegrationTests;

[Collection(Constants.DefaultFixtureName)]
[Trait("Category", "Integration")]
public class AllFlagTests
{
    private readonly DefaultFixture _fixture;

    public AllFlagTests(DefaultFixture fixture)
    {
        _fixture = fixture;
    }

    [IntegrationFact]
    public async Task IntegrationTest_ValidOutput_WithAllShortFlag()
    {
        using var process = _fixture.GetBbranchProcess("-a");

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
    public async Task IntegrationTest_ValidOutput_WithAllLongFlag()
    {
        using var process = _fixture.GetBbranchProcess("--all");

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
    public async Task IntegrationTest_InvalidOutput_WithAllFlagAndValue()
    {
        using var process = _fixture.GetBbranchProcess("--all", "value");

        var (output, error) = await _fixture.RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        output = output.Replace("\r", "");

        Assert.Equal("fatal: Value for --all is not allowed\n", output);
    }

    [IntegrationFact]
    public async Task IntegrationTest_InvalidOutput_WithAllAndRemoteFlag()
    {
        using var process = _fixture.GetBbranchProcess("--all", "--remote");

        var (output, error) = await _fixture.RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        output = output.Replace("\r", "");

        Assert.Equal("fatal: Cannot use both --all and --remote\n", output);
    }
}
