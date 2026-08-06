using System.Globalization;
using System.Text.RegularExpressions;

using Bbranch.Tests.Helpers;
using Bbranch.Tests.Infrastucture.Fixtures;

namespace Bbranch.Tests.Integration;

[Trait("Category", "Integration")]
public class SortFlagTests
{
    private readonly TestExecutor _sut;

    public SortFlagTests(DefaultFixture fixture)
    {
        _sut = new TestExecutor(fixture);
    }

    [Fact]
    public async Task IntegrationTest_ValidOutput_WithSortShortFlagAndNameValue()
    {
        var result = await _sut.ExecuteWithShortFlagAndName();

        string[] lines = result.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        AssertHelper.Header(lines);

        foreach (string line in lines.Skip(2))
        {
            var (ahead, behind) = AheadBehindHelper.GetAheadBehind(line);

            Assert.True(ahead >= 0, $"ahead was below 0... Actual: {ahead}... Line: {line}");
            Assert.True(behind >= 0, $"behind was below 0... Actual: {behind} Line: {line}");
        }

        string[] branchNames = lines
            .Skip(2)
            .Select(l => l.Split('|')[2].Trim())
            .ToArray();

        string[] sortedBranchNames = [.. branchNames.OrderBy(b => b)];

        Assert.Equal(branchNames, sortedBranchNames);
    }

    [Fact]
    public async Task IntegrationTest_ValidOutput_WithSortLongFlagAndNameValue()
    {
        var result = await _sut.ExecuteWithLongFlagAndName();

        string[] lines = result.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        AssertHelper.Header(lines);

        foreach (string line in lines.Skip(2))
        {
            var (ahead, behind) = AheadBehindHelper.GetAheadBehind(line);

            Assert.True(ahead >= 0, $"ahead was below 0... Actual: {ahead}... Line: {line}");
            Assert.True(behind >= 0, $"behind was below 0... Actual: {behind} Line: {line}");
        }

        string[] branchNames = lines
            .Skip(2)
            .Select(l => l.Split('|')[2].Trim())
            .ToArray();

        string[] sortedBranchNames = [.. branchNames.OrderBy(b => b)];

        Assert.Equal(branchNames, sortedBranchNames);
    }

    [Fact]
    public async Task IntegrationTest_ValidOutput_WithSortShortFlagAndAheadValue()
    {
        var result = await _sut.ExecuteWithShortFlagAndAhead();

        string[] lines = result.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        AssertHelper.Header(lines);

        foreach (string line in lines.Skip(2))
        {
            var (ahead, behind) = AheadBehindHelper.GetAheadBehind(line);

            Assert.True(ahead >= 0, $"ahead was below 0... Actual: {ahead}... Line: {line}");
            Assert.True(behind >= 0, $"behind was below 0... Actual: {behind} Line: {line}");
        }

        var aheadValues =
            lines.Skip(2);

        var sortedAheadValues = aheadValues.OrderByDescending(a => a);

        Assert.Equal(aheadValues, aheadValues);
    }

    [Fact]
    public async Task IntegrationTest_ValidOutput_WithSortLongFlagAndAheadValue()
    {
        var result = await _sut.ExecuteWithLongFlagAndAhead();

        string[] lines = result.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        AssertHelper.Header(lines);

        foreach (string line in lines.Skip(2))
        {
            var (ahead, behind) = AheadBehindHelper.GetAheadBehind(line);

            Assert.True(ahead >= 0, $"ahead was below 0... Actual: {ahead}... Line: {line}");
            Assert.True(behind >= 0, $"behind was below 0... Actual: {behind} Line: {line}");
        }

        var aheadValues =
            lines.Skip(2);

        var sortedAheadValues = aheadValues.OrderByDescending(a => a);

        Assert.Equal(aheadValues, sortedAheadValues);
    }

    [Fact]
    public async Task IntegrationTest_ValidOutput_WithSortShortFlagAndBehindValue()
    {
        var result = await _sut.ExecuteWithShortFlagAndBehind();

        string[] lines = result.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        AssertHelper.Header(lines);

        foreach (string line in lines.Skip(2))
        {
            var (ahead, behind) = AheadBehindHelper.GetAheadBehind(line);

            Assert.True(ahead >= 0, $"ahead was below 0... Actual: {ahead}... Line: {line}");
            Assert.True(behind >= 0, $"behind was below 0... Actual: {behind} Line: {line}");
        }

        var behindValues =
            lines.Skip(2);

        var sortedBehindValues = behindValues.OrderByDescending(b => b);

        Assert.Equal(behindValues, sortedBehindValues);
    }

    [Fact]
    public async Task IntegrationTest_ValidOutput_WithSortLongFlagAndBehindValue()
    {
        var result = await _sut.ExecuteWithLongFlagAndBehind();

        string[] lines = result.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        AssertHelper.Header(lines);

        foreach (string line in lines.Skip(2))
        {
            var (ahead, behind) = AheadBehindHelper.GetAheadBehind(line);

            Assert.True(ahead >= 0, $"ahead was below 0... Actual: {ahead}... Line: {line}");
            Assert.True(behind >= 0, $"behind was below 0... Actual: {behind} Line: {line}");
        }

        var behindValues =
            lines.Skip(2);

        var sortedBehindValues = behindValues.OrderByDescending(b => b);

        Assert.Equal(behindValues, sortedBehindValues);
    }

    [Fact]
    public async Task IntegrationTest_InvalidOutput_WithSortFlagAndInvalidValue()
    {
        var result = await _sut.ExecuteWithInvalidValue();

        Assert.Equal(
            "fatal: '--sort' must a criterion of 'date', 'name', 'ahead', or 'behind'\n",
            result
        );
    }

    [Fact]
    public async Task IntegrationTest_ValidOutput_WithSortShortFlagAndDateValue()
    {
        var result = await _sut.ExecuteWithShortFlagAndDate();

        string[] lines = result.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        AssertHelper.Header(lines);

        foreach (string line in lines.Skip(2))
        {
            var (ahead, behind) = AheadBehindHelper.GetAheadBehind(line);

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

        string[] sortedCommitDates = commitDates
            .Select(dateStr => new
            {
                OriginalString = dateStr,
                DateTime = ParseRelativeDate(CleanSpaces(dateStr)),
                OrderPriority = GetOrderPriority(dateStr),
            })
            .OrderBy(x => x.OrderPriority)
            .ThenByDescending(x => x.DateTime)
            .Select(x => x.OriginalString)
            .ToArray();

        Assert.Equal(sortedCommitDates, commitDates);
    }

    [Fact]
    public async Task IntegrationTest_ValidOutput_WithSortLongFlagAndDateValue()
    {
        var result = await _sut.ExecuteWithLongFlagAndDate();

        string[] lines = result.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        AssertHelper.Header(lines);

        foreach (string line in lines.Skip(2))
        {
            var (ahead, behind) = AheadBehindHelper.GetAheadBehind(line);

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

        string[] sortedCommitDates = commitDates
            .Select(dateStr => new
            {
                OriginalString = dateStr,
                DateTime = ParseRelativeDate(CleanSpaces(dateStr)),
                OrderPriority = GetOrderPriority(dateStr),
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
            DateTime parsedTime = DateTime.ParseExact(
                timePart,
                "HH:mm",
                CultureInfo.InvariantCulture
            );
            return new DateTime(
                now.Year,
                now.Month,
                now.Day - 1,
                parsedTime.Hour,
                parsedTime.Minute,
                0
            );
        }
        else if (dateStr.Contains("today"))
        {
            string timePart = dateStr.Split(' ')[0];
            DateTime parsedTime = DateTime.ParseExact(
                timePart,
                "HH:mm",
                CultureInfo.InvariantCulture
            );
            return new DateTime(
                now.Year,
                now.Month,
                now.Day,
                parsedTime.Hour,
                parsedTime.Minute,
                0
            );
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

    private class TestExecutor(DefaultFixture fixture)
    {
        public async Task<string> ExecuteWithShortFlagAndName() => await fixture.Exec("-s", "name");
        public async Task<string> ExecuteWithLongFlagAndName() => await fixture.Exec("--sort", "name");
        public async Task<string> ExecuteWithShortFlagAndAhead() => await fixture.Exec("-s", "ahead");
        public async Task<string> ExecuteWithLongFlagAndAhead() => await fixture.Exec("--sort", "ahead");
        public async Task<string> ExecuteWithShortFlagAndBehind() => await fixture.Exec("-s", "behind");
        public async Task<string> ExecuteWithLongFlagAndBehind() => await fixture.Exec("--sort", "behind");
        public async Task<string> ExecuteWithInvalidValue() => await fixture.Exec("-s", "invalid");
        public async Task<string> ExecuteWithShortFlagAndDate() => await fixture.Exec("-s", "date");
        public async Task<string> ExecuteWithLongFlagAndDate() => await fixture.Exec("--sort", "date");
    }
}
