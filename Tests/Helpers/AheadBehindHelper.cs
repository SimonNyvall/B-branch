using System.Text.RegularExpressions;

namespace Bbranch.Tests.Helpers;

public static partial class AheadBehindHelper
{
    public static (int ahead, int behind) GetAheadBehind(string value)
    {
        var match = AheadBehindPattern().Match(value);

        if (!match.Success)
        {
            throw new Exception($"Failed to parse ahead/behind... Line: {value}");
        }

        return (int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
    }

    [GeneratedRegex(@"\s*(\d+)\s*\|\s*(\d+)")]
    private static partial Regex AheadBehindPattern();
}
