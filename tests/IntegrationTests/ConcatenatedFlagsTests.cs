namespace Bbranch.IntegrationTests;

[Collection("Sequential")]
public class ConcatenatedFlagsTests : IntegrationBase
{
    [Fact(Timeout = 120000)]
    public async Task IntegrationTest_ValidOutput_WithAllAndQuietFlagsConcatenated()
    {
        using var process = GetBbranchProcessWithoutPager("-qa");

        var (output, error) = await RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        string[] lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        Assert.DoesNotContain("Ahead 󰜘", lines[0]);
        Assert.DoesNotContain("Behind 󰜘", lines[0]);
        Assert.DoesNotContain("Branch Name ", lines[0]);
        Assert.DoesNotContain("Last commit ", lines[0]);

        Assert.Contains(lines, l => l.Contains("origin"));
        Assert.Contains(lines, l => !l.Contains("origin"));
    }

    [Fact(Timeout = 120000)]
    public async Task IntegrationTest_ValidOutput_WithAllAndQuietAndSortFlagsConcatenated()
    {
        using var process = GetBbranchProcessWithoutPager("-qa", "--sort", "date");

        var (output, error) = await RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        string[] lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        Assert.DoesNotContain("Ahead 󰜘", lines[0]);
        Assert.DoesNotContain("Behind 󰜘", lines[0]);
        Assert.DoesNotContain("Branch Name ", lines[0]);
        Assert.DoesNotContain("Last commit ", lines[0]);

        Assert.Contains(lines, l => l.Contains("origin"));
        Assert.Contains(lines, l => !l.Contains("origin"));
    }

    [Fact(Timeout = 120000)]
    public async Task IntegrationTest_ValidOutput_WithRemoteAndQuietFlagConcatenated()
    {
        using var process = GetBbranchProcessWithoutPager("-rq");

        var (output, error) = await RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        string[] lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        Assert.DoesNotContain("Ahead 󰜘", lines[0]);
        Assert.DoesNotContain("Behind 󰜘", lines[0]);
        Assert.DoesNotContain("Branch Name ", lines[0]);
        Assert.DoesNotContain("Last commit ", lines[0]);

        Assert.All(lines, l => l.Contains("origin"));
    }

    [Fact(Timeout = 120000)]
    public async Task IntegrationTest_InvalidOutput_WithDoubleDashAndQuietAndAllFlagConcatenated()
    {
        using var process = GetBbranchProcessWithoutPager("--qa");

        var (output, error) = await RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        output = output.Replace("\r", "");

        Assert.Equal("Only one dash is allowed: --qa\n", output);
    }

    [Fact(Timeout = 120000)]
    public async Task IntegrationTest_InvalidOutput_WithDuplicateFlagsConcatenated()
    {
        using var process = GetBbranchProcessWithoutPager("-qaq");

        var (output, error) = await RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        output = output.Replace("\r", "");

        Assert.Equal("Duplicate option: -q\n", output);
    }

    [Fact(Timeout = 120000)]
    public async Task IntegrationTest_InvalidOutput_WithUnkownFlagConcatenated()
    {
        using var process = GetBbranchProcessWithoutPager("-qz");

        var (output, error) = await RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        output = output.Replace("\r", "");

        Assert.Equal("Invalid option: -z\n", output);
    }
}