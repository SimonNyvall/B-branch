using System.Text.RegularExpressions;
using System.Globalization;

namespace Bbranch.IntegrationTests;

internal partial class CommonTests
{
    internal static void AssertHeader(string[] headerLines)
    {
        Assert.True(headerLines.Length >= 2, "Header lines does not contain enought lines for header print.");

        Assert.Contains("Ahead 󰜘", headerLines[0]);
        Assert.Contains("Behind 󰜘", headerLines[0]);
        Assert.Contains("Branch Name ", headerLines[0]);
        Assert.Contains("Last commit ", headerLines[0]);

        Assert.True(headerLines[1].All(c => c == '|' || c == '-' || c == ' '));
    }

    internal static (int ahead, int behind) GetAheadBehindFromString(string line)
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

