using System.Text.RegularExpressions;

namespace Bbranch.IntegrationTests;

[Collection(Constants.DefaultFixtureName)]
public class VersionFlagTests
{
    private readonly DefaultFixture _fixture;

    public VersionFlagTests(DefaultFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task IntegrationTest_ValidOutput_WithVersionShortFlag()
    {
        using var process = _fixture.GetBbranchProcess("-v");

        var (output, error) = await _fixture.RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        const string pattern = @"v(\d+)\.(\d+)\.(\d+)";

        Match match = Regex.Match(output, pattern);

        Assert.True(match.Success, "Failed to match version pattern.");
    }

    [Fact]
    public async Task IntegrationTest_ValidOutput_WithVersionLongFlag()
    {
        using var process = _fixture.GetBbranchProcess("-v");

        var (output, error) = await _fixture.RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        const string pattern = @"v(\d+)\.(\d+)\.(\d+)";

        Match match = Regex.Match(output, pattern);

        Assert.True(match.Success, "Failed to match version pattern.");
    }

    [Fact]
    public async Task IntegrationTest_InvalidOutput_WithVersionFlagAndValue()
    {
        using var process = _fixture.GetBbranchProcess("--version", "value");

        var (output, error) = await _fixture.RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        output = output.Replace("\r", "");

        Assert.Equal("fatal: Value for --version is not allowed\n", output);
    }

    [Fact]
    public async Task IntegrationTest_InvalidOutput_WithVersionFlagAndOtherFlag()
    {
        using var process = _fixture.GetBbranchProcess("--version", "-a");

        var (output, error) = await _fixture.RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        output = output.Replace("\r", "");

        Assert.Equal("fatal: --version cannot be used with any other option\n", output);
    }
}
