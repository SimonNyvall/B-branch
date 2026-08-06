namespace Bbranch.Tests.Helpers;

public static class AssertHelper
{
    public static void Header(string[] lines)
    {
        Assert.True(
            lines.Length >= 2,
            $"Header lines does not contain enought lines for header print. Expected at least 2 lines. Actual: {lines.Length} Lines: {string.Join('\n', lines)}"
        );

        Assert.Contains("Ahead", lines[0]);
        Assert.Contains("Behind", lines[0]);
        Assert.Contains("Branch name", lines[0]);
        Assert.Contains("Last commit", lines[0]);

        Assert.True(lines[1].All(c => c == '|' || c == '-' || c == ' ' || c == '\r'));
    }
}
