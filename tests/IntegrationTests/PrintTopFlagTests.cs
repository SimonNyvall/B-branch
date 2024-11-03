namespace Bbranch.IntegrationTests;

public class PrintTopFlagTests
{
    [Fact]
    public async Task IntegrationTest_ValidOutput_WithPrintTopFlag()
    {
        await IntegrationTest_ValidOutput_WithPrintTopShortFlag();
        await IntegrationTest_ValidOutput_WithPrintTopLongFlag();
    }

    private static async Task IntegrationTest_ValidOutput_WithPrintTopLongFlag()
    {
        using var process = ProcessHelper.GetDotnetProcess("--print-top", "1");
        process.Start();

        string output = await process.StandardOutput.ReadToEndAsync();
        string error = await process.StandardError.ReadToEndAsync();

        process.WaitForExit();

        Assert.True(string.IsNullOrEmpty(error), error);

        string[] lines = output.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        CommonTests.AssertHeader(lines);

        Assert.True(lines.Length <= 3, "Too many lines printed.");
    }

    private static async Task IntegrationTest_ValidOutput_WithPrintTopShortFlag()
    {
        using var process = ProcessHelper.GetDotnetProcess("-p", "1");
        process.Start();

        string output = await process.StandardOutput.ReadToEndAsync();
        string error = await process.StandardError.ReadToEndAsync();

        process.WaitForExit();

        Assert.True(string.IsNullOrEmpty(error), error);

        string[] lines = output.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        CommonTests.AssertHeader(lines);

        Assert.True(lines.Length <= 3, "Too many lines printed.");
    }
}