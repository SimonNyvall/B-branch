using Bbranch.Tests.Infrastucture.Fixtures;

namespace Bbranch.Tests.Integration;

[Trait("Category", "Integration")]
public class QuietFlagTests
{
    private readonly TestExecutor _sut;

    public QuietFlagTests(DefaultFixture fixture)
    {
        _sut = new TestExecutor(fixture);
    }

    [Fact]
    public async Task IntegrationTest_ValidOutput_WithQuietShortFlag()
    {
        var result = await _sut.ExecuteWithShortFlag();

        string[] lines = result.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        Assert.DoesNotContain("Ahead 󰜘", lines[0]);
        Assert.DoesNotContain("Behind 󰜘", lines[0]);
        Assert.DoesNotContain("Branch Name ", lines[0]);
        Assert.DoesNotContain("Last commit ", lines[0]);
    }

    [Fact]
    public async Task IntegrationTest_ValidOutput_WithQuietLongFlag()
    {
        var result = await _sut.ExecuteWithLongFlag();

        string[] lines = result.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        Assert.DoesNotContain("Ahead 󰜘", lines[0]);
        Assert.DoesNotContain("Behind 󰜘", lines[0]);
        Assert.DoesNotContain("Branch Name ", lines[0]);
        Assert.DoesNotContain("Last commit ", lines[0]);
    }

    [Fact]
    public async Task IntegrationTest_InvalidOutput_WithQuietFlagAndValue()
    {
        var result = await _sut.ExecuteWithValue();

        Assert.Equal("fatal: Value for --quiet is not allowed\n", result);
    }

    private class TestExecutor(DefaultFixture fixture)
    {
        public async Task<string> ExecuteWithShortFlag() => await fixture.Exec("-q");

        public async Task<string> ExecuteWithLongFlag() => await fixture.Exec("--quiet");

        public async Task<string> ExecuteWithValue() => await fixture.Exec("--quiet", "value");
    }
}
