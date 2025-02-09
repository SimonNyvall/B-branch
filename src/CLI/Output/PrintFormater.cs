using System.Globalization;

namespace Bbranch.CLI.Output;

public static class PrintFormater
{
    public static string GetTimePrefix(DateTime lastCommit, DateTime currentTime)
    {
        CultureInfo culture = CultureInfo.InvariantCulture;
        DateTimeFormatInfo dateTimeFormat = culture.DateTimeFormat;

        int days = (currentTime - lastCommit).Days;

        if (days == 0)
        {
            string timeFormat;

            if (dateTimeFormat.ShortTimePattern.Contains("tt"))
            {
                timeFormat = (lastCommit.Hour > 9 && lastCommit.Hour < 13) ? "h:mm tt" : "h:mm  tt";
            }
            else
            {
                timeFormat = "HH:mm";
            }

            string time = lastCommit.ToString(timeFormat, culture);

            return $"{time} today";
        }

        if (days == 1)
        {
            string time = lastCommit.ToString("HH:mm", culture);
            return $"{time} yesterday";
        }

        if (days >= 30)
        {
            DateTime pastDate = currentTime.AddDays(-days);
            int monthDifference = ((currentTime.Year - pastDate.Year) * 12) + currentTime.Month - pastDate.Month;

            return monthDifference > 1 ? $"{monthDifference}     months ago" : "1     month ago";
        }

        string timeElapsed = days == 1 ? "day" : "days";

        int padLeft = 5 - days.ToString().Length;
        return $"{days} {new string(' ', padLeft)}{timeElapsed} ago";
    }
}