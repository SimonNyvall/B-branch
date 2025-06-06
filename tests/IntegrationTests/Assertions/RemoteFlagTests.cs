namespace Bbranch.IntegrationTests;

[Collection("Sequential")]
public class RemoteFlagTests : IntegrationBase
{
    [Fact(Timeout = 120000)]
    public async Task IntegrationTest_ValidOutput_WithRemoteShortFlag()
    {
        using var process = GetBbranchProcessWithoutPager("-r");
      
        var (output, error) = await RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        string[] lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        AssertHeader(lines);

        foreach (string line in lines.Skip(2))
        {
            var (ahead, behind) = GetAheadBehindFromString(line);

            Assert.True(ahead >= 0, $"ahead was below 0... Actual: {ahead}... Line: {line}");
            Assert.True(behind >= 0, $"behind was below 0... Actual: {behind} Line: {line}");
        }
    }

    [Fact(Timeout = 120000)]
    public async Task IntegrationTest_ValidOutput_WithRemoteLongFlag()
    {
        using var process = GetBbranchProcessWithoutPager("--remote");

        var (output, error) = await RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        string[] lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        AssertHeader(lines);

        foreach (string line in lines.Skip(2))
        {
            var (ahead, behind) = GetAheadBehindFromString(line);

            Assert.True(ahead >= 0, $"ahead was below 0... Actual: {ahead}... Line: {line}");
            Assert.True(behind >= 0, $"behind was below 0... Actual: {behind} Line: {line}");
        }
    }

    [Fact(Timeout = 120000)]
    public async Task IntegrationTest_InvalidOutput_WithRemoteFlagAndValue()
    {
        using var process = GetBbranchProcessWithoutPager("--remote", "value");

        var (output, error) = await RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        output = output.Replace("\r", "");

        Assert.Equal("fatal: Value for --remote is not allowed\n", output);
    }

    [Fact(Timeout = 120000)]
    public async Task IntegrationTest_InvalidOutput_WithRemoteAndAllFlag()
    {
        using var process = GetBbranchProcessWithoutPager("--remote", "--all");

        var (output, error) = await RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        output = output.Replace("\r", "");

        Assert.Equal("fatal: Cannot use both --all and --remote\n", output);
    }
}