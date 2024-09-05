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

    #region HelpFlag
    [Fact]
    public async Task IntegrationTest_ValidOutput_WithHelpFlag()
    {
        await IntegrationTest_ValidOutput_WithHelpShortFlag();
        await IntegrationTest_ValidOutput_WithHelpLongFlag();
    }

    private async Task IntegrationTest_ValidOutput_WithHelpShortFlag()
    {
        using var process = ProcessHelper.GetDotnetProcess("-h");
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

    private async Task IntegrationTest_ValidOutput_WithHelpLongFlag()
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
    #endregion

    #region VersionFlag    
    [Fact]
    public async Task IntegrationTest_ValidOutput_WithVersionFlag()
    {
        await IntegrationTest_ValidOutput_WithVersionShortFlag();
        await IntegrationTest_ValidOutput_WithVersionLongFlag();
    }

    private async Task IntegrationTest_ValidOutput_WithVersionShortFlag()
    {
        using var process = ProcessHelper.GetDotnetProcess("-v");
        process.Start();

        string output = await process.StandardOutput.ReadToEndAsync();
        string error = await process.StandardError.ReadToEndAsync();

        Assert.True(string.IsNullOrEmpty(error), error);

        const string pattern = @"v(\d+)\.(\d+)\.(\d+)";

        Match match = Regex.Match(output, pattern);

        Assert.True(match.Success, "Failed to match version pattern.");
    }

    private async Task IntegrationTest_ValidOutput_WithVersionLongFlag()
    {
        using var process = ProcessHelper.GetDotnetProcess("-v");
        process.Start();

        string output = await process.StandardOutput.ReadToEndAsync();
        string error = await process.StandardError.ReadToEndAsync();

        Assert.True(string.IsNullOrEmpty(error), error);

        const string pattern = @"v(\d+)\.(\d+)\.(\d+)";

        Match match = Regex.Match(output, pattern);

        Assert.True(match.Success, "Failed to match version pattern.");
    }
    #endregion

    #region TrackFlag
    [Fact]
    public async Task IntegrationTest_ValidOutput_WithTrackFlag()
    {
        await IntegrationTest_ValidOutput_WithTrackShortFlag();
        await IntegrationTest_ValidOutput_WithTrackLongFlag();
    }

    private async Task IntegrationTest_ValidOutput_WithTrackShortFlag()
    {
        using var process = ProcessHelper.GetDotnetProcess("-t main");
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

    private async Task IntegrationTest_ValidOutput_WithTrackLongFlag()
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

    #endregion

    #region SortFlag
    [Fact]
    public async Task IntegrationTest_ValidOutput_WithSortFlag()
    {
        await IntegrationTest_ValidOutput_WithSortShortFlag();
        await IntegrationTest_ValidOutput_WithSortLongFlag();
    }

    private async Task IntegrationTest_ValidOutput_WithSortShortFlag()
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

    private async Task IntegrationTest_ValidOutput_WithSortLongFlag()
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

    #endregion

    #region ContainsFlag
    [Fact]
    public async Task IntegrationTest_ValidOutput_WithContainsFlag()
    {
        await IntegrationTest_ValidOutput_WithContainsShortFlag();
        await IntegrationTest_ValidOutput_WithContainsLongFlag();
    }

    private async Task IntegrationTest_ValidOutput_WithContainsShortFlag()
    {
        using var process = ProcessHelper.GetDotnetProcess("-c main");
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

    private async Task IntegrationTest_ValidOutput_WithContainsLongFlag()
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
    #endregion

    #region NoContainsFlag
    [Fact]
    public async Task IntegrationTest_ValidOutput_WithNoContainsFlag()
    {
        await IntegrationTest_ValidOutput_WithNoContainsShortFlag();
        await IntegrationTest_ValidOutput_WithNoContainsLongFlag();
    }

    private async Task IntegrationTest_ValidOutput_WithNoContainsShortFlag()
    {
        using var process = ProcessHelper.GetDotnetProcess("-n main");
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

        Assert.All(branchNames, b => Assert.DoesNotContain("main", b, StringComparison.OrdinalIgnoreCase));
    }

    private async Task IntegrationTest_ValidOutput_WithNoContainsLongFlag()
    {
        using var process = ProcessHelper.GetDotnetProcess("--no-contains main");
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

        Assert.All(branchNames, b => Assert.DoesNotContain("main", b, StringComparison.OrdinalIgnoreCase));
    }
    #endregion

    #region AllFlag
    [Fact]
    public async Task IntegrationTest_ValidOutput_WithAllFlag()
    {
        await IntegrationTest_ValidOutput_WithAllShortFlag();
        await IntegrationTest_ValidOutput_WithAllLongFlag();
    }

    private async Task IntegrationTest_ValidOutput_WithAllShortFlag()
    {
        using var process = ProcessHelper.GetDotnetProcess("-a");
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

    private async Task IntegrationTest_ValidOutput_WithAllLongFlag()
    {
        using var process = ProcessHelper.GetDotnetProcess("--all");
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
    #endregion

    #region RemoteFlag
    [Fact]
    public async Task IntegrationTest_ValidOutput_WithRemoteFlag()
    {
        await IntegrationTest_ValidOutput_WithRemoteShortFlag();
        await IntegrationTest_ValidOutput_WithRemoteLongFlag();
    }

    private async Task IntegrationTest_ValidOutput_WithRemoteShortFlag()
    {
        using var process = ProcessHelper.GetDotnetProcess("-r");
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

    private async Task IntegrationTest_ValidOutput_WithRemoteLongFlag()
    {
        using var process = ProcessHelper.GetDotnetProcess("--remote");
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

    #endregion

    #region QuietFlag
    [Fact]
    public async Task IntegrationTest_ValidOutput_WithQuietFlag()
    {
        await IntegrationTest_ValidOutput_WithQuietShortFlag();
        await IntegrationTest_ValidOutput_WithQuietLongFlag();
    }

    private async Task IntegrationTest_ValidOutput_WithQuietShortFlag()
    {
        using var process = ProcessHelper.GetDotnetProcess("-q");
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

    private async Task IntegrationTest_ValidOutput_WithQuietLongFlag()
    {
        using var process = ProcessHelper.GetDotnetProcess("--quiet");
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

    #endregion

    #region PrintTopFlag
    [Fact]
    public async Task IntegrationTest_ValidOutput_WithPrintTopFlag()
    {
        await IntegrationTest_ValidOutput_WithPrintTopShortFlag();
        await IntegrationTest_ValidOutput_WithPrintTopLongFlag();
    }

    private async Task IntegrationTest_ValidOutput_WithPrintTopLongFlag()
    {
        using var process = ProcessHelper.GetDotnetProcess("--print-top 1");
        process.Start();

        string output = await process.StandardOutput.ReadToEndAsync();
        string error = await process.StandardError.ReadToEndAsync();

        process.WaitForExit();

        Assert.True(string.IsNullOrEmpty(error), error);

        string[] lines = output.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        AssertHeader(lines);

        Assert.True(lines.Length <= 3, "Too many lines printed.");
    }

    private async Task IntegrationTest_ValidOutput_WithPrintTopShortFlag()
    {
        using var process = ProcessHelper.GetDotnetProcess("-p 1");
        process.Start();

        string output = await process.StandardOutput.ReadToEndAsync();
        string error = await process.StandardError.ReadToEndAsync();

        process.WaitForExit();

        Assert.True(string.IsNullOrEmpty(error), error);

        string[] lines = output.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        AssertHeader(lines);

        Assert.True(lines.Length <= 3, "Too many lines printed.");
    }
    #endregion

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