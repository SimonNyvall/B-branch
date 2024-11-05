using System.Globalization;
using System.Text.RegularExpressions;

namespace Bbranch.IntegrationTests;

[Collection("Sequential")]
public class SortFlagTests : IntegrationBase
{
    [Fact(Timeout = 120000)]
    public async Task IntegrationTest_ValidOutput_WithSortShortFlagAndNameValue()
    {
        using var process = GetBbranchProcessWithoutPager("-s", "name");

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

        string[] branchNames = lines.Skip(2).Select(l => l.Split('|')[2].Trim()).ToArray();

        string[] sortedBranchNames = [.. branchNames.OrderBy(b => b)];

        Assert.Equal(branchNames, sortedBranchNames);
    }

    [Fact(Timeout = 120000)]
    public async Task IntegrationTest_ValidOutput_WithSortLongFlagAndNameValue()
    {
        using var process = GetBbranchProcessWithoutPager("--sort", "name");

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

        string[] branchNames = lines.Skip(2).Select(l => l.Split('|')[2].Trim()).ToArray();

        string[] sortedBranchNames = [.. branchNames.OrderBy(b => b)];

        Assert.Equal(branchNames, sortedBranchNames);
    }

    [Fact(Timeout = 120000)]
    public async Task IntegrationTest_ValidOutput_WithSortShortFlagAndDateValue()
    {
        using var process = GetBbranchProcessWithoutPager("-s", "date");

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

        string[] commitDates = lines
            .Skip(2)
            .Select(l =>
            {
                string dateWithDescription = l.Split('|')[3].Trim();
                int agoIndex = dateWithDescription.IndexOf("ago") + 3;

                if (agoIndex > 2)
                {
                    return dateWithDescription.Substring(0, agoIndex).Trim();
                }

                return dateWithDescription;
            })
            .ToArray();

        string[] sortedCommitDates = commitDates.Select(dateStr => new
        {
            OriginalString = dateStr,
            DateTime = ParseRelativeDate(CleanSpaces(dateStr)),
            OrderPriority = GetOrderPriority(dateStr)
        })
       .OrderBy(x => x.OrderPriority)
       .ThenByDescending(x => x.DateTime)
       .Select(x => x.OriginalString)
       .ToArray();

        Assert.Equal(sortedCommitDates, commitDates);
    }

    [Fact(Timeout = 120000)]
    public async Task IntegrationTest_ValidOutput_WithSortLongFlagAndDateValue()
    {
        using var process = GetBbranchProcessWithoutPager("--sort", "date");

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

        string[] commitDates = lines
            .Skip(2)
            .Select(l =>
            {
                string dateWithDescription = l.Split('|')[3].Trim();
                int agoIndex = dateWithDescription.IndexOf("ago") + 3;

                if (agoIndex > 2)
                {
                    return dateWithDescription.Substring(0, agoIndex).Trim();
                }

                return dateWithDescription;
            })
            .ToArray();

        string[] sortedCommitDates = commitDates.Select(dateStr => new
        {
            OriginalString = dateStr,
            DateTime = ParseRelativeDate(CleanSpaces(dateStr)),
            OrderPriority = GetOrderPriority(dateStr)
        })
       .OrderBy(x => x.OrderPriority)
       .ThenByDescending(x => x.DateTime)
       .Select(x => x.OriginalString)
       .ToArray();

        Assert.Equal(sortedCommitDates, commitDates);
    }

    [Fact(Timeout = 120000)]
    public async Task IntegrationTest_ValidOutput_WithSortShortFlagAndAheadValue()
    {
        using var process = GetBbranchProcessWithoutPager("-s", "ahead");

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

        int[] aheadValues = lines.Skip(2).Select(l => int.Parse(l.Split('|')[0].Trim())).ToArray();

        int[] sortedAheadValues = [.. aheadValues.OrderBy(a => a)];

        Assert.Equal(aheadValues, sortedAheadValues);
    }

    [Fact(Timeout = 120000)]
    public async Task IntegrationTest_ValidOutput_WithSortLongFlagAndAheadValue()
    {
        using var process = GetBbranchProcessWithoutPager("--sort", "ahead");

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

        int[] aheadValues = lines.Skip(2).Select(l => int.Parse(l.Split('|')[0].Trim())).ToArray();

        int[] sortedAheadValues = [.. aheadValues.OrderBy(a => a)];

        Assert.Equal(aheadValues, sortedAheadValues);
    }

    [Fact(Timeout = 120000)]
    public async Task IntegrationTest_ValidOutput_WithSortShortFlagAndBehindValue()
    {
        using var process = GetBbranchProcessWithoutPager("-s", "behind");

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

        int[] behindValues = lines.Skip(2).Select(l => int.Parse(l.Split('|')[1].Trim())).ToArray();

        int[] sortedBehindValues = [.. behindValues.OrderBy(b => b)];

        Assert.Equal(behindValues, sortedBehindValues);
    }

    [Fact(Timeout = 120000)]
    public async Task IntegrationTest_ValidOutput_WithSortLongFlagAndBehindValue()
    {
        using var process = GetBbranchProcessWithoutPager("--sort", "behind");

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

        int[] behindValues = lines.Skip(2).Select(l => int.Parse(l.Split('|')[1].Trim())).ToArray();

        int[] sortedBehindValues = [.. behindValues.OrderBy(b => b)];

        Assert.Equal(behindValues, sortedBehindValues);
    }

    private static string CleanSpaces(string input)
    {
        return Regex.Replace(input.Trim(), @"\s+", " ");
    }

    private static DateTime ParseRelativeDate(string dateStr)
    {
        DateTime now = DateTime.Now;

        if (dateStr.Contains("yesterday"))
        {
            string timePart = dateStr.Split(' ')[0];
            DateTime parsedTime = DateTime.ParseExact(timePart, "HH:mm", CultureInfo.InvariantCulture);
            return new DateTime(now.Year, now.Month, now.Day - 1, parsedTime.Hour, parsedTime.Minute, 0);
        }
        else if (dateStr.Contains("today"))
        {
            string timePart = dateStr.Split(' ')[0];
            DateTime parsedTime = DateTime.ParseExact(timePart, "HH:mm", CultureInfo.InvariantCulture);
            return new DateTime(now.Year, now.Month, now.Day, parsedTime.Hour, parsedTime.Minute, 0);
        }
        else if (dateStr.Contains("day ago") || dateStr.Contains("days ago"))
        {
            int daysAgo = int.Parse(dateStr.Split(' ')[0]);
            return now.AddDays(-daysAgo);
        }
        else
        {
            throw new ArgumentException("Unknown date format: " + dateStr);
        }
    }

    private static int GetOrderPriority(string dateStr)
    {
        if (dateStr.Contains("today"))
            return 1;
        else if (dateStr.Contains("yesterday"))
            return 2;
        else if (dateStr.Contains("day ago"))
            return 3;
        else if (dateStr.Contains("days ago"))
            return 4;
        else
            return 5; // Default for unknown formats
    }
}