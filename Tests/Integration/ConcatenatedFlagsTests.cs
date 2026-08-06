using Bbranch.Tests.Infrastucture.Fixtures;

namespace Bbranch.Tests.Integration;

[Trait("Category", "Integration")]
public class ConcatenatedFlagsTests
{
    private readonly TestExecutor _sut;

    public ConcatenatedFlagsTests(DefaultFixture fixture)
    {
        _sut = new TestExecutor(fixture);
    }

    [Fact]
    public async Task IntegrationTest_ValidOutput_WithAllAndQuietFlagsConcatenated()
    {
        var result = await _sut.ExecuteWithAllAndQuietFlagsConcatenated();

        result = result.Replace("*", "");
        string[] lines = result.Split(' ', StringSplitOptions.RemoveEmptyEntries);

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
        var result = await _sut.ExecuteWithRemoteAndQuietFlagConcatenated();

        string[] lines = result.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        Assert.DoesNotContain("Ahead 󰜘", lines[0]);
        Assert.DoesNotContain("Behind 󰜘", lines[0]);
        Assert.DoesNotContain("Branch Name ", lines[0]);
        Assert.DoesNotContain("Last commit ", lines[0]);

        Assert.All(lines, l => l.Contains("origin"));
    }

    [Fact]
    public async Task IntegrationTest_InvalidOutput_WithDoubleDashAndQuietAndAllFlagConcatenated()
    {
        var result = await _sut.ExecuteWithDoubleDashAndQuietAndAllFlagConcatenated();

        string[] outputLines = result.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        Assert.Equal(
            "error: pathspec '--qa' did not match any file(s) known to git",
            outputLines[0]
        );
    }

    [Fact]
    public async Task IntegrationTest_InvalidOutput_WithDuplicateFlagsConcatenated()
    {
        var result = await _sut.ExecuteWithDuplicateFlagsConcatenated();

        string[] outputLines = result.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        Assert.Equal("error: duplicated option: -q", outputLines[0]);
    }

    [Fact]
    public async Task IntegrationTest_InvalidOutput_WithUnkownFlagConcatenated()
    {
        var result = await _sut.ExecuteWithUnkownFlagConcatenated();

        string[] outputLines = result.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        Assert.Equal("error: unknown option: -z", outputLines[0]);
    }

    private class TestExecutor(DefaultFixture fixture)
    {
        public async Task<string> ExecuteWithAllAndQuietFlagsConcatenated() =>
            await fixture.Exec("-qa");

        public async Task<string> ExecuteWithRemoteAndQuietFlagConcatenated() =>
            await fixture.Exec("-rq");

        public async Task<string> ExecuteWithDoubleDashAndQuietAndAllFlagConcatenated() =>
            await fixture.Exec("--qa");

        public async Task<string> ExecuteWithDuplicateFlagsConcatenated() =>
            await fixture.Exec("-qaq");

        public async Task<string> ExecuteWithUnkownFlagConcatenated() => await fixture.Exec("-qz");
    }
}
