using System.Text.RegularExpressions;
using System.Globalization;

namespace Bbranch.IntegrationTests;

[Collection("Sequential")]
public class NoFlagTests : IntegrationBase
{
    [Fact(Timeout = 120000)]
    public async Task IntegrationTest_ValidOutput_WithNoFlags()
    {
        using var process = GetBbranchProcessWithoutPager();

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

