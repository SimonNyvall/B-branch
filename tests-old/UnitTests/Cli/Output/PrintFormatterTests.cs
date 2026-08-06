using System.Globalization;
using Bbranch.CLI.Output;

namespace Bbranch.Tests.CLI.Output;

[Trait("Category", "Unit")]
public sealed class PrintFormatterTests
{
    private readonly DateTime _currentTime = new(2020, 1, 1, 12, 0, 0);

    [Fact]
    public void Given_PrintFormatter_When_GetTimePrefixRun_Then_Return_Today_WithSameTime()
    {
        DateTime dateTime = _currentTime;
        var expected = $"{dateTime.ToString("HH:mm", CultureInfo.CurrentCulture)} today";

        var actual = PrintFormatter.GetTimePrefix(dateTime, _currentTime);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Given_PrintFormatter_When_GetTimePrefixRun_Then_Return_YesterdayPrefix_WithOneDayDifference()
    {
        DateTime dateTime = _currentTime.AddDays(-1);
        var expected = $"{dateTime.ToString("HH:mm", CultureInfo.CurrentCulture)} yesterday";

        var actual = PrintFormatter.GetTimePrefix(dateTime, _currentTime);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(-2)]
    [InlineData(-5)]
    [InlineData(-9)]
    [InlineData(-10)]
    [InlineData(-15)]
    public void Given_PrintFormatter_When_GetTimePrefixRun_Then_Return_DaysPrefix_WithMultipleDaysDifference(
        int backoff
    )
    {
        DateTime dateTime = _currentTime.AddDays(backoff);

        var actual = PrintFormatter.GetTimePrefix(dateTime, _currentTime);

        if (backoff <= -10)
        {
            var expected = $"{backoff * -1}    days ago";
            Assert.Equal(expected, actual);
        }
        else
        {
            var expected = $"{backoff * -1}     days ago";
            Assert.Equal(expected, actual);
        }
    }

    [Fact]
    public void Given_PrintFormatter_When_GetTimePrefixRun_Then_Return_MonthPrefix_WithMultipleDaysDifference()
    {
        DateTime dateTime = _currentTime.AddDays(-31);
        var expected = $"1     month ago";

        var actual = PrintFormatter.GetTimePrefix(dateTime, _currentTime);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Given_PrintFormatter_When_GetTimePrefixRun_Then_Return_Months_WithMultipleDaysDifference()
    {
        DateTime dateTime = _currentTime.AddDays(-60);
        var expected = $"2     months ago";

        var actual = PrintFormatter.GetTimePrefix(dateTime, _currentTime);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Given_PrintFormatter_When_GetTimePrefixRun_Then_Return_Months_WithYearDifference()
    {
        DateTime dateTime = _currentTime.AddYears(-1);
        var expected = $"12    months ago";

        var actual = PrintFormatter.GetTimePrefix(dateTime, _currentTime);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Given_PrintFormatter_When_GetTimePrefixRun_Then_Return_Months_WithYearsDifference()
    {
        DateTime dateTime = _currentTime.AddYears(-2);
        var expected = $"24    months ago";

        var actual = PrintFormatter.GetTimePrefix(dateTime, _currentTime);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Given_PrintFormatter_When_GetTimePrefixRun_Then_Return_Dashes_WithLargeYearDifference()
    {
        DateTime dateTime = _currentTime.AddYears(-1600);
        var expected = "--";

        var actual = PrintFormatter.GetTimePrefix(dateTime, _currentTime);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Given_PrintFormatter_When_GetTimePrefixRun_Then_Return_Months_WithDaysPassingMonths()
    {
        DateTime dateTime = _currentTime.AddDays(-40);
        var expected = "1     month ago";

        var actual = PrintFormatter.GetTimePrefix(dateTime, _currentTime);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Given_PrintFormatter_When_GetTimePrefixRun_Then_Return_Months_WithManyMonths()
    {
        DateTime dateTime = _currentTime.AddMonths(-11);
        var expected = "11    months ago";

        var actual = PrintFormatter.GetTimePrefix(dateTime, _currentTime);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Given_PrintFormatter_When_GetTimePrefixRun_Then_Return_Months_WithOneAndAHalfMonth()
    {
        DateTime dateTime = _currentTime.AddDays(-30 - 15);
        var expected = "2     months ago";

        var actual = PrintFormatter.GetTimePrefix(dateTime, _currentTime);

        Assert.Equal(expected, actual);
    }
}
