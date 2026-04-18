namespace Bbranch.IntegrationTests;

[Collection(Constants.DefaultFixtureName)]
[Trait("Category", "Integration")]
public class ConcatenatedFlagsTests
{
    private readonly DefaultFixture _fixture;

    public ConcatenatedFlagsTests(DefaultFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task IntegrationTest_ValidOutput_WithAllAndQuietFlagsConcatenated()
    {
        using var process = _fixture.GetBbranchProcess("-qa");

        var (output, error) = await _fixture.RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        output = output.Replace("*", "");
        string[] lines = output.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        Assert.DoesNotContain("Ahead 󰜘", lines[0]);
        Assert.DoesNotContain("Behind 󰜘", lines[0]);
        Assert.DoesNotContain("Branch Name ", lines[0]);
        Assert.DoesNotContain("Last commit ", lines[0]);

        Assert.Equal(5, lines.Length);
        Assert.Contains(lines, l => l.Contains("origin"));
        Assert.Contains(lines, l => !l.Contains("origin"));
    }

    [Fact]
    public async Task IntegrationTest_ValidOutput_WithRemoteAndQuietFlagConcatenated()
    {
        using var process = _fixture.GetBbranchProcess("-rq");

        var (output, error) = await _fixture.RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        string[] lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        Assert.DoesNotContain("Ahead 󰜘", lines[0]);
        Assert.DoesNotContain("Behind 󰜘", lines[0]);
        Assert.DoesNotContain("Branch Name ", lines[0]);
        Assert.DoesNotContain("Last commit ", lines[0]);

        Assert.All(lines, l => l.Contains("origin"));
    }

    [Fact]
    public async Task IntegrationTest_InvalidOutput_WithDoubleDashAndQuietAndAllFlagConcatenated()
    {
        using var process = _fixture.GetBbranchProcess("--qa");

        var (output, error) = await _fixture.RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        output = output.Replace("\r", "");
        string[] outputLines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        Assert.Equal(
            "error: pathspec '--qa' did not match any file(s) known to git",
            outputLines[0]
        );
    }

    [Fact]
    public async Task IntegrationTest_InvalidOutput_WithDuplicateFlagsConcatenated()
    {
        using var process = _fixture.GetBbranchProcess("-qaq");

        var (output, error) = await _fixture.RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        output = output.Replace("\r", "");
        string[] outputLines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        Assert.Equal("error: duplicated option: -q", outputLines[0]);
    }

    [Fact]
    public async Task IntegrationTest_InvalidOutput_WithUnkownFlagConcatenated()
    {
        using var process = _fixture.GetBbranchProcess("-qz");

        var (output, error) = await _fixture.RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        output = output.Replace("\r", "");
        string[] outputLines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        Assert.Equal("error: unknown option: -z", outputLines[0]);
    }
}
