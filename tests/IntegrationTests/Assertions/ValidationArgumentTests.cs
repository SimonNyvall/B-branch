namespace Bbranch.IntegrationTests;

[Collection("Sequential")]
public class ValidationArgumentsTests : IntegrationBase
{
    [Fact(Timeout = 120000)]
    public async Task IntegrationTest_NotValidOutput_WithVersionAndOtherFlag()
    {
        using var process = GetBbranchProcessWithoutPager("-v", "-t", "main");
      
        var (output, error) = await RunProcessWithTimeoutAsync(process);

        output = output.Replace("\r", "");

        Assert.True(string.IsNullOrEmpty(error), error);
        Assert.Equal("You cannot use --version with any other option\n", output);
    }

    [Fact(Timeout = 120000)]
    public async Task IntegrationTest_NotValidOutput_WithContainsAndNoContainsFlag()
    {
        using var process = GetBbranchProcessWithoutPager("-c", "main", "-n", "main");

        var (output, error) = await RunProcessWithTimeoutAsync(process);

        output = output.Replace("\r", "");

        Assert.True(string.IsNullOrEmpty(error), error);
        Assert.Equal("You cannot use both --contains and --no-contains\n", output);
    }

    [Fact(Timeout = 120000)]
    public async Task IntegrationTest_NotValidOutput_WithAllAndRemoteFlag()
    {
        using var process = GetBbranchProcessWithoutPager("-a", "-r");

        var (output, error) = await RunProcessWithTimeoutAsync(process);

        output = output.Replace("\r", "");

        Assert.True(string.IsNullOrEmpty(error), error);
        Assert.Equal("You cannot use both --all and --remote\n", output);
    }

    [Fact(Timeout = 120000)]
    public async Task IntegrationTest_NotValidOutput_WithPrintTopFlag()
    {
        using var process = GetBbranchProcessWithoutPager("-p", "0");

        var (output, error) = await RunProcessWithTimeoutAsync(process);

        output = output.Replace("\r", "");

        Assert.True(string.IsNullOrEmpty(error), error);
        Assert.Equal("Value for --print-top must be greater than 0\n", output);
    }
}