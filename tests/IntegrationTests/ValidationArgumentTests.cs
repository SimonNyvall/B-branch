namespace Bbranch.IntegrationTests;

public class ValidationArgumentsTests : IntegrationBase
{
    [Fact]
    public async Task IntegrationTest_NotValidOutput_WithVersionAndOtherFlag()
    {
        using var process = GetDotnetProcess("-v", "-t", "main");
        process.Start();

        string output = await process.StandardOutput.ReadToEndAsync();
        string error = await process.StandardError.ReadToEndAsync();

        output = output.Replace("\r", "");

        Assert.True(string.IsNullOrEmpty(error), error);
        Assert.Equal("You cannot use --version with any other option\n", output);
    }

    [Fact]
    public async Task IntegrationTest_NotValidOutput_WithContainsAndNoContainsFlag()
    {
        using var process = GetDotnetProcess("-c", "main", "-n", "main");
        process.Start();

        string output = await process.StandardOutput.ReadToEndAsync();
        string error = await process.StandardError.ReadToEndAsync();

        output = output.Replace("\r", "");

        Assert.True(string.IsNullOrEmpty(error), error);
        Assert.Equal("You cannot use both --contains and --no-contains\n", output);
    }

    [Fact]
    public async Task IntegrationTest_NotValidOutput_WithAllAndRemoteFlag()
    {
        using var process = GetDotnetProcess("-a", "-r");
        process.Start();

        string output = await process.StandardOutput.ReadToEndAsync();
        string error = await process.StandardError.ReadToEndAsync();

        output = output.Replace("\r", "");

        Assert.True(string.IsNullOrEmpty(error), error);
        Assert.Equal("You cannot use both --all and --remote\n", output);
    }

    [Fact]
    public async Task IntegrationTest_NotValidOutput_WithPrintTopFlag()
    {
        using var process = GetDotnetProcess("-p", "0");
        process.Start();

        string output = await process.StandardOutput.ReadToEndAsync();
        string error = await process.StandardError.ReadToEndAsync();

        output = output.Replace("\r", "");

        Assert.True(string.IsNullOrEmpty(error), error);
        Assert.Equal("Value for --print-top must be greater than 0\n", output);
    }
}