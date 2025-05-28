using System.Globalization;
using Bbranch.CLI.Output;

namespace Bbranch.Tests.CLI.Output;

public sealed class PrintFormaterTests
{
    private readonly DateTime _currentTime = new(2020, 1, 1, 12, 0, 0);

    [Fact]
    public void Given_PrintFormater_When_GetTimePrefixRun_Then_Return_Today_WithSameTime()
    {
        DateTime dateTime = _currentTime;
        var expected = $"{dateTime.ToString("HH:mm", CultureInfo.CurrentCulture)} today";

        var actual = PrintFormater.GetTimePrefix(dateTime, _currentTime);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Given_PrintFormater_When_GetTimePrefixRun_Then_Return_YesterdayPrefix_WithOneDayDifference()
    {
        DateTime dateTime = _currentTime.AddDays(-1);
        var expected = $"{dateTime.ToString("HH:mm", CultureInfo.CurrentCulture)} yesterday";

        var actual = PrintFormater.GetTimePrefix(dateTime, _currentTime);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Given_PrintFormater_When_GetTimePrefixRun_Then_Return_DaysPrefix_WithMultipleDaysDifference()
    {
        DateTime dateTime = _currentTime.AddDays(-5);
        var expected = $"5     days ago";

        var actual = PrintFormater.GetTimePrefix(dateTime, _currentTime);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Given_PrintFormater_When_GetTimePrefixRun_Then_Return_MonthPrefix_WithMultipleDaysDifference()
    {
        DateTime dateTime = _currentTime.AddDays(-31);
        var expected = $"1     month ago";

        var actual = PrintFormater.GetTimePrefix(dateTime, _currentTime);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Given_PrintFormater_When_GetTimePrefixRun_Then_Return_Months_WithMultipleDaysDifference()
    {
        DateTime dateTime = _currentTime.AddDays(-60);
        var expected = $"2     months ago";

        var actual = PrintFormater.GetTimePrefix(dateTime, _currentTime);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Given_PrintFormater_When_GetTimePrefixRun_Then_Return_Months_WithYearDifference()
    {
        DateTime dateTime = _currentTime.AddYears(-1);
        var expected = $"12     months ago";

        var actual = PrintFormater.GetTimePrefix(dateTime, _currentTime);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Given_PrintFormater_When_GetTimePrefixRun_Then_Return_Months_WithYearsDifference()
    {
        DateTime dateTime = _currentTime.AddYears(-2);
        var expected = $"24     months ago";

        var actual = PrintFormater.GetTimePrefix(dateTime, _currentTime);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Given_PrintFormater_When_GetTimePrefixRun_Then_Return_Dashes_WithLargeYearDifference()
    {
        DateTime dateTime = _currentTime.AddYears(-1600);
        var expected = "--";

        var actual = PrintFormater.GetTimePrefix(dateTime, _currentTime);

        Assert.Equal(expected, actual);
    }
}