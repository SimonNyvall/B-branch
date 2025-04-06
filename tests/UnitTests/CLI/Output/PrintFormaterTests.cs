using System.Globalization;
using Bbranch.CLI.Output;

namespace Bbranch.Tests.CLI.Output;

public class PrintFormaterTests
{
    private readonly DateTime _currentTime = new(2020, 1, 1, 12, 0, 0);

    [Fact]
    public void GetTimePrefix_ShouldReturnToday_WithSameTime()
    {
        DateTime dateTime = _currentTime;
        string expected = $"{dateTime.ToString("HH:mm", CultureInfo.CurrentCulture)} today";

        string actual = PrintFormater.GetTimePrefix(dateTime, _currentTime);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetTimePrefix_ShouldReturnYesterdayPrefix_WithOneDayDifference()
    {
        DateTime dateTime = _currentTime.AddDays(-1);
        string expected = $"{dateTime.ToString("HH:mm", CultureInfo.CurrentCulture)} yesterday";

        string actual = PrintFormater.GetTimePrefix(dateTime, _currentTime);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetTimePrefix_ShouldReturnDaysPrefix_WithMultipleDaysDifference()
    {
        DateTime dateTime = _currentTime.AddDays(-5);
        string expected = $"5     days ago";

        string actual = PrintFormater.GetTimePrefix(dateTime, _currentTime);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetTimePrefix_ShouldReturnMonthPrefix_WithMultipleDaysDifference()
    {
        DateTime dateTime = _currentTime.AddDays(-31);
        string expected = $"1     month ago";

        string actual = PrintFormater.GetTimePrefix(dateTime, _currentTime);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetTimePrefix_ShouldReturnMonths_WithMultipleDaysDifference()
    {
        DateTime dateTime = _currentTime.AddDays(-60);
        string expected = $"2     months ago";

        string actual = PrintFormater.GetTimePrefix(dateTime, _currentTime);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetTimePrefix_ShouldReturnMonths_WithYearDifference()
    {
        DateTime dateTime = _currentTime.AddYears(-1);
        string expected = $"12     months ago";

        string actual = PrintFormater.GetTimePrefix(dateTime, _currentTime);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetTimePrefix_ShouldReturnMonths_WithYearsDifference()
    {
        DateTime dateTime = _currentTime.AddYears(-2);
        string expected = $"24     months ago";

        string actual = PrintFormater.GetTimePrefix(dateTime, _currentTime);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetTimePrefix_ShouldReturnDashes_WithLargeYearDifference()
    {
        DateTime dateTime = _currentTime.AddYears(-1600);
        string expected = "--";
        
        string actual = PrintFormater.GetTimePrefix(dateTime, _currentTime);
        
        Assert.Equal(expected, actual);
    }
}