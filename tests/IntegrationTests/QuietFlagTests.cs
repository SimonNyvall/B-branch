namespace Bbranch.IntegrationTests;

public class QuietFlagTests : IntegrationBase
{
    [Fact]
    public async Task IntegrationTest_ValidOutput_WithQuietFlag()
    {
        await IntegrationTest_ValidOutput_WithQuietShortFlag();
        await IntegrationTest_ValidOutput_WithQuietLongFlag();
    }

    private static async Task IntegrationTest_ValidOutput_WithQuietShortFlag()
    {
        using var process = GetDotnetProcess("-q");
        process.Start();

        string output = await process.StandardOutput.ReadToEndAsync();
        string error = await process.StandardError.ReadToEndAsync();

        process.WaitForExit();

        Assert.True(string.IsNullOrEmpty(error), error);

        string[] lines = output.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        Assert.DoesNotContain("Ahead 󰜘", lines[0]);
        Assert.DoesNotContain("Behind 󰜘", lines[0]);
        Assert.DoesNotContain("Branch Name ", lines[0]);
        Assert.DoesNotContain("Last commit ", lines[0]);
    }

    private static async Task IntegrationTest_ValidOutput_WithQuietLongFlag()
    {
        using var process = GetDotnetProcess("--quiet");
        process.Start();

        string output = await process.StandardOutput.ReadToEndAsync();
        string error = await process.StandardError.ReadToEndAsync();

        process.WaitForExit();

        Assert.True(string.IsNullOrEmpty(error), error);

        string[] lines = output.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        Assert.DoesNotContain("Ahead 󰜘", lines[0]);
        Assert.DoesNotContain("Behind 󰜘", lines[0]);
        Assert.DoesNotContain("Branch Name ", lines[0]);
        Assert.DoesNotContain("Last commit ", lines[0]);
    }
}