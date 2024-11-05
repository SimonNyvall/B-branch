namespace Bbranch.IntegrationTests;

[Collection("Sequential")]
public class ConcatenatedFlagsTests : IntegrationBase
{
    [Fact(Timeout = 120000)]
    public async Task IntegrationTest_ValidOutput_WithAllAndQuiteFlagsConcatenated()
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
    } // TODO: add more tests here
}