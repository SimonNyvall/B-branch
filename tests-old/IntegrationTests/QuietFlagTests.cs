namespace Bbranch.IntegrationTests;

[Collection(Constants.DefaultFixtureName)]
[Trait("Category", "Integration")]
public class QuietFlagTests
{
    private readonly DefaultFixture _fixture;

    public QuietFlagTests(DefaultFixture fixture)
    {
        _fixture = fixture;
    }

    [IntegrationFact]
    public async Task IntegrationTest_ValidOutput_WithQuietShortFlag()
    {
        using var process = _fixture.GetBbranchProcess("-q");

        var (output, error) = await _fixture.RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        string[] lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        Assert.DoesNotContain("Ahead 󰜘", lines[0]);
        Assert.DoesNotContain("Behind 󰜘", lines[0]);
        Assert.DoesNotContain("Branch Name ", lines[0]);
        Assert.DoesNotContain("Last commit ", lines[0]);
    }

    [IntegrationFact]
    public async Task IntegrationTest_ValidOutput_WithQuietLongFlag()
    {
        using var process = _fixture.GetBbranchProcess("--quiet");

        var (output, error) = await _fixture.RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        string[] lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        Assert.DoesNotContain("Ahead 󰜘", lines[0]);
        Assert.DoesNotContain("Behind 󰜘", lines[0]);
        Assert.DoesNotContain("Branch Name ", lines[0]);
        Assert.DoesNotContain("Last commit ", lines[0]);
    }

    [IntegrationFact]
    public async Task IntegrationTest_InvalidOutput_WithQuietFlagAndValue()
    {
        using var process = _fixture.GetBbranchProcess("--quiet", "value");

        var (output, error) = await _fixture.RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        output = output.Replace("\r", "");

        Assert.Equal("fatal: Value for --quiet is not allowed\n", output);
    }
}
