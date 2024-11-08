namespace Bbranch.IntegrationTests;

[Collection("Sequential")]
public class PagerFlagTests : IntegrationBase
{
    [Fact(Timeout = 120000)]
    public async Task IntegrationTest_InvalidOutput_WithPagerAndNoPagerFlag()
    {
        // The --no-pager flag is already applied in the process
        using var process = GetBbranchProcessWithoutPager("--pager");

        var (output, error) = await RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        output = output.Replace("\r", "");

        Assert.Equal("fatal: Cannot use both --pager and --no-pager\n", output);
    }
}