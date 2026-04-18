using System.Globalization;
using System.Text.RegularExpressions;

namespace Bbranch.IntegrationTests;

[Collection(Constants.DefaultFixtureName)]
public class SortFlagTests
{
    private readonly DefaultFixture _fixture;

    public SortFlagTests(DefaultFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task IntegrationTest_ValidOutput_WithSortShortFlagAndNameValue()
    {
        using var process = _fixture.GetBbranchProcess("-s", "name");

        var (output, error) = await _fixture.RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        string[] lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        _fixture.AssertHeader(lines);

        foreach (string line in lines.Skip(2))
        {
            var (ahead, behind) = _fixture.GetAheadBehindFromString(line);

            Assert.True(ahead >= 0, $"ahead was below 0... Actual: {ahead}... Line: {line}");
            Assert.True(behind >= 0, $"behind was below 0... Actual: {behind} Line: {line}");
        }

        string[] branchNames = lines
            .Skip(2)
            .Select(l => l.Split('|')[2].Trim())
            .Select(l => _fixture.RemoveUnixChars(l).Trim())
            .ToArray();

        string[] sortedBranchNames = [.. branchNames.OrderBy(b => b)];

        Assert.Equal(branchNames, sortedBranchNames);
    }

    [Fact]
    public async Task IntegrationTest_ValidOutput_WithSortLongFlagAndNameValue()
    {
        using var process = _fixture.GetBbranchProcess("--sort", "name");

        var (output, error) = await _fixture.RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        string[] lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        _fixture.AssertHeader(lines);

        foreach (string line in lines.Skip(2))
        {
            var (ahead, behind) = _fixture.GetAheadBehindFromString(line);

            Assert.True(ahead >= 0, $"ahead was below 0... Actual: {ahead}... Line: {line}");
            Assert.True(behind >= 0, $"behind was below 0... Actual: {behind} Line: {line}");
        }

        string[] branchNames = lines
            .Skip(2)
            .Select(l => l.Split('|')[2].Trim())
            .Select(l => _fixture.RemoveUnixChars(l).Trim())
            .ToArray();

        string[] sortedBranchNames = [.. branchNames.OrderBy(b => b)];

        Assert.Equal(branchNames, sortedBranchNames);
    }

    [Fact]
    public async Task IntegrationTest_ValidOutput_WithSortShortFlagAndAheadValue()
    {
        using var process = _fixture.GetBbranchProcess("-s", "ahead");

        var (output, error) = await _fixture.RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        string[] lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        _fixture.AssertHeader(lines);

        foreach (string line in lines.Skip(2))
        {
            var (ahead, behind) = _fixture.GetAheadBehindFromString(line);

            Assert.True(ahead >= 0, $"ahead was below 0... Actual: {ahead}... Line: {line}");
            Assert.True(behind >= 0, $"behind was below 0... Actual: {behind} Line: {line}");
        }

        int[] aheadValues =
        [
            .. lines.Skip(2).Select(l => _fixture.GetAheadBehindFromString(l).ahead),
        ];

        int[] sortedAheadValues = [.. aheadValues.OrderByDescending(a => a)];

        Assert.Equal(aheadValues, sortedAheadValues);
    }

    [Fact]
    public async Task IntegrationTest_ValidOutput_WithSortLongFlagAndAheadValue()
    {
        using var process = _fixture.GetBbranchProcess("--sort", "ahead");

        var (output, error) = await _fixture.RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        string[] lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        _fixture.AssertHeader(lines);

        foreach (string line in lines.Skip(2))
        {
            var (ahead, behind) = _fixture.GetAheadBehindFromString(line);

            Assert.True(ahead >= 0, $"ahead was below 0... Actual: {ahead}... Line: {line}");
            Assert.True(behind >= 0, $"behind was below 0... Actual: {behind} Line: {line}");
        }

        int[] aheadValues =
        [
            .. lines.Skip(2).Select(l => _fixture.GetAheadBehindFromString(l).ahead),
        ];

        int[] sortedAheadValues = [.. aheadValues.OrderByDescending(a => a)];

        Assert.Equal(aheadValues, sortedAheadValues);
    }

    [Fact]
    public async Task IntegrationTest_ValidOutput_WithSortShortFlagAndBehindValue()
    {
        using var process = _fixture.GetBbranchProcess("-s", "behind");

        var (output, error) = await _fixture.RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        string[] lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        _fixture.AssertHeader(lines);

        foreach (string line in lines.Skip(2))
        {
            var (ahead, behind) = _fixture.GetAheadBehindFromString(line);

            Assert.True(ahead >= 0, $"ahead was below 0... Actual: {ahead}... Line: {line}");
            Assert.True(behind >= 0, $"behind was below 0... Actual: {behind} Line: {line}");
        }

        int[] behindValues =
        [
            .. lines.Skip(2).Select(l => _fixture.GetAheadBehindFromString(l).behind),
        ];

        int[] sortedBehindValues = [.. behindValues.OrderByDescending(b => b)];

        Assert.Equal(behindValues, sortedBehindValues);
    }

    [Fact]
    public async Task IntegrationTest_ValidOutput_WithSortLongFlagAndBehindValue()
    {
        using var process = _fixture.GetBbranchProcess("--sort", "behind");

        var (output, error) = await _fixture.RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        string[] lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        _fixture.AssertHeader(lines);

        foreach (string line in lines.Skip(2))
        {
            var (ahead, behind) = _fixture.GetAheadBehindFromString(line);

            Assert.True(ahead >= 0, $"ahead was below 0... Actual: {ahead}... Line: {line}");
            Assert.True(behind >= 0, $"behind was below 0... Actual: {behind} Line: {line}");
        }

        int[] behindValues =
        [
            .. lines.Skip(2).Select(l => _fixture.GetAheadBehindFromString(l).behind),
        ];

        int[] sortedBehindValues = [.. behindValues.OrderByDescending(b => b)];

        Assert.Equal(behindValues, sortedBehindValues);
    }

    [Fact]
    public async Task IntegrationTest_InvalidOutput_WithSortFlagAndInvalidValue()
    {
        using var process = _fixture.GetBbranchProcess("-s", "invalid");

        var (output, error) = await _fixture.RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        output = output.Replace("\r", "");

        Assert.Equal(
            "fatal: '--sort' must a criterion of 'date', 'name', 'ahead', or 'behind'\n",
            output
        );
    }

    [Fact]
    public async Task IntegrationTest_ValidOutput_WithSortShortFlagAndDateValue()
    {
        using var process = _fixture.GetBbranchProcess("-s", "date");

        var (output, error) = await _fixture.RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        string[] lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        _fixture.AssertHeader(lines);

        foreach (string line in lines.Skip(2))
        {
            var (ahead, behind) = _fixture.GetAheadBehindFromString(line);

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
        using var process = _fixture.GetBbranchProcess("--sort", "date");

        var (output, error) = await _fixture.RunProcessWithTimeoutAsync(process);

        Assert.True(string.IsNullOrEmpty(error), error);

        string[] lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        _fixture.AssertHeader(lines);

        foreach (string line in lines.Skip(2))
        {
            var (ahead, behind) = _fixture.GetAheadBehindFromString(line);

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
}
