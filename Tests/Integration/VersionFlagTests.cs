using System.Text.RegularExpressions;
using Bbranch.Tests.Infrastucture.Fixtures;

namespace Bbranch.Tests.Integration;

[Trait("Category", "Integration")]
public class VersionFlagTests
{
    private readonly TestExecutor _sut;

    public VersionFlagTests(DefaultFixture fixture)
    {
        _sut = new TestExecutor(fixture);
    }

    [Fact]
    public async Task IntegrationTest_ValidOutput_WithVersionShortFlag()
    {
        const string pattern = @"v(\d+)\.(\d+)\.(\d+)";

        var result = await _sut.ExecuteWithShortFlag();

        Match match = Regex.Match(result, pattern);

        Assert.True(match.Success, "Failed to match version pattern.");
    }

    [Fact]
    public async Task IntegrationTest_ValidOutput_WithVersionLongFlag()
    {
        const string pattern = @"v(\d+)\.(\d+)\.(\d+)";

        var result = await _sut.ExcuteWithLongFlag();

        Match match = Regex.Match(result, pattern);

        Assert.True(match.Success, "Failed to match version pattern.");
    }

    [Fact]
    public async Task IntegrationTest_InvalidOutput_WithVersionFlagAndValue()
    {
        var result = await _sut.ExcuteWithLongFlagAndValue();

        result = result.Replace("\r", "");

        Assert.Equal("fatal: Value for --version is not allowed\n", result);
    }

    [Fact]
    public async Task IntegrationTest_InvalidOutput_WithVersionFlagAndOtherFlag()
    {
        var result = await _sut.ExecuteWithShortFlagAndValue();

        result = result.Replace("\r", "");

        Assert.Equal("fatal: --version cannot be used with any other option\n", result);
    }

    private class TestExecutor(DefaultFixture fixture)
    {
        public async Task<string> ExecuteWithShortFlag()
        {
            var result = await fixture.Exec(fixture.PublishedArtifactPath, "-v");
            return result.Stdout;
        }

        public async Task<string> ExcuteWithLongFlag()
        {
            var result = await fixture.Exec(fixture.PublishedArtifactPath, "--version");
            return result.Stdout;
        }

        public async Task<string> ExecuteWithShortFlagAndValue()
        {
            var result = await fixture.Exec(fixture.PublishedArtifactPath, "-v", "value");
            return result.Stdout;
        }

        public async Task<string> ExcuteWithLongFlagAndValue()
        {
            var result = await fixture.Exec(fixture.PublishedArtifactPath, "--version", "-a");
            return result.Stdout;
        }
    }
}
