namespace Bbranch.IntegrationTests;

public class QuietFlagTests : IntegrationBase
{
    [Fact(Timeout = 120000)]
    public async Task IntegrationTest_ValidOutput_WithQuietFlag()
    {
        await IntegrationTest_ValidOutput_WithQuietShortFlag();
        await IntegrationTest_ValidOutput_WithQuietLongFlag();
    }

    private async Task IntegrationTest_ValidOutput_WithQuietShortFlag()
    {
        using var process = GetDotnetProcess("-q");
       
        var (output, error) = await RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        string[] lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        Assert.DoesNotContain("Ahead 󰜘", lines[0]);
        Assert.DoesNotContain("Behind 󰜘", lines[0]);
        Assert.DoesNotContain("Branch Name ", lines[0]);
        Assert.DoesNotContain("Last commit ", lines[0]);
    }

    private async Task IntegrationTest_ValidOutput_WithQuietLongFlag()
    {
        using var process = GetDotnetProcess("--quiet");

        var (output, error) = await RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        string[] lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        Assert.DoesNotContain("Ahead 󰜘", lines[0]);
        Assert.DoesNotContain("Behind 󰜘", lines[0]);
        Assert.DoesNotContain("Branch Name ", lines[0]);
        Assert.DoesNotContain("Last commit ", lines[0]);
    }
}