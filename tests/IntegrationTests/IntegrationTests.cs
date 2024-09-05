using System.Text.RegularExpressions;
using System.Globalization;

namespace IntegrationTests;

public partial class IntegrationTest
{
    [Fact]
    public async Task IntegrationTest_ValidOutput_WithNoFlags()
    {
        using var process = ProcessHelper.GetDotnetProcess();
        process.Start();

        string output = await process.StandardOutput.ReadToEndAsync();
        string error = await process.StandardError.ReadToEndAsync();

        process.WaitForExit();

        Assert.True(string.IsNullOrEmpty(error), error);

        string[] lines = output.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
    
        AssertHeader(lines);

        foreach (string line in lines.Skip(2))
        {
            var (ahead, behind) = GetAheadBehindFromString(line);

            Assert.True(ahead >= 0, "ahead was below 0.");
            Assert.True(behind >= 0, "behind was below 0.");
        }
    }

    [Fact]
    public async Task IntegrationTest_ValidOutput_WithHelpFlag()
    {
        using var process = ProcessHelper.GetDotnetProcess("--help");
        process.Start();

        string output = await process.StandardOutput.ReadToEndAsync();
        string error = await process.StandardError.ReadToEndAsync();

        Assert.True(string.IsNullOrEmpty(error), error);

        string[] lines = output.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        Assert.Contains("--help,        -h", lines[2]);
        Assert.Contains("--track,       -t", lines[3]);
        Assert.Contains("--sort,        -s", lines[4]);
        Assert.Contains("--contains,    -c", lines[5]);
        Assert.Contains("--no-contains, -n", lines[6]);
        Assert.Contains("--all,         -a", lines[7]);
        Assert.Contains("--remote,      -r", lines[8]);
        Assert.Contains("--quiet,       -q", lines[9]);
        Assert.Contains("--print-top,   -p", lines[10]);
        Assert.Contains("--version,     -v", lines[11]);
    }

    [Fact]
    public async Task IntegrationTest_ValidOutput_WithVersionFlag()
    {
        using var process = ProcessHelper.GetDotnetProcess("--version");
        process.Start();

        string output = await process.StandardOutput.ReadToEndAsync();
        string error = await process.StandardError.ReadToEndAsync();

        Assert.True(string.IsNullOrEmpty(error), error);

        const string pattern = @"v(\d+)\.(\d+)\.(\d+)";

        Match match = Regex.Match(output, pattern);

        Assert.True(match.Success, "Failed to match version pattern.");
    }

    [Fact]
    public async Task IntegrationTest_ValidOutput_WithTrackFlag()
    {
        using var process = ProcessHelper.GetDotnetProcess("--track main");
        process.Start();

        string output = await process.StandardOutput.ReadToEndAsync();
        string error = await process.StandardError.ReadToEndAsync();

        process.WaitForExit();

        Assert.True(string.IsNullOrEmpty(error), error);

        string[] lines = output.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        AssertHeader(lines);

        foreach (string line in lines.Skip(2))
        {
            var (ahead, behind) = GetAheadBehindFromString(line);

            Assert.True(ahead >= 0, "ahead was below 0.");
            Assert.True(behind >= 0, "behind was below 0.");
        }
    }

    [Fact]
    public async Task IntegrationTest_ValidOutput_WithSortFlag()
    {
        using var process = ProcessHelper.GetDotnetProcess("--sort name");
        process.Start();

        string output = await process.StandardOutput.ReadToEndAsync();
        string error = await process.StandardError.ReadToEndAsync();

        process.WaitForExit();

        Assert.True(string.IsNullOrEmpty(error), error);

        string[] lines = output.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        AssertHeader(lines);

        foreach (string line in lines.Skip(2))
        {
            var (ahead, behind) = GetAheadBehindFromString(line);

            Assert.True(ahead >= 0, "ahead was below 0.");
            Assert.True(behind >= 0, "behind was below 0.");
        }

        string[] branchNames = lines.Skip(2).Select(l => l.Split('|')[2].Trim()).ToArray();

        string[] sortedBranchNames = branchNames.OrderBy(b => b).ToArray();

        Assert.Equal(branchNames, sortedBranchNames);
    }

    [Fact]
    public async Task IntegrationTest_ValidOutput_WithContainsFlag()
    {
        using var process = ProcessHelper.GetDotnetProcess("--contains main");
        process.Start();

        string output = await process.StandardOutput.ReadToEndAsync();
        string error = await process.StandardError.ReadToEndAsync();

        process.WaitForExit();

        Assert.True(string.IsNullOrEmpty(error), error);

        string[] lines = output.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        AssertHeader(lines);

        foreach (string line in lines.Skip(2))
        {
            var (ahead, behind) = GetAheadBehindFromString(line);

            Assert.True(ahead >= 0, "ahead was below 0.");
            Assert.True(behind >= 0, "behind was below 0.");
        }

        string[] branchNames = lines.Skip(2).Select(l => l.Split('|')[2].Trim()).ToArray();

        Assert.All(branchNames, b => Assert.Contains("main", b, StringComparison.OrdinalIgnoreCase));
    }

    private static void AssertHeader(string[] headerLines)
    {
        Assert.True(headerLines.Length >= 2, "Header lines does not contain enought lines for header print.");

        Assert.Contains("Ahead 󰜘", headerLines[0]);
        Assert.Contains("Behind 󰜘", headerLines[0]);
        Assert.Contains("Branch Name ", headerLines[0]);
        Assert.Contains("Last commit ", headerLines[0]);

        Assert.True(headerLines[1].All(c => c == '|' || c == '-' || c == ' '));
    }

    private static (int ahead, int behind) GetAheadBehindFromString(string line)
    {
        int ahead = 0;
        int behind = 0;

        Match match = aheadBehindPattern().Match(line);

        if (!match.Success) throw new Exception("Failed to parse ahead/behind");
        
        ahead = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
        behind = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);

        return (ahead, behind);
    }

    [GeneratedRegex(@"\s*(\d+)\s*\|\s*(\d+)", RegexOptions.Compiled)]
    private static partial Regex aheadBehindPattern();
}