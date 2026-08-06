namespace Bbranch.CLI.Output;

public static class PrintFormatter
{
    private const string FiveSpacer = "     ";
    private const string FourSpacer = "    ";

    public static string GetTimePrefix(DateTime lastCommit, DateTime currentTime)
    {
        if (lastCommit.Year <= 1601)
            return "--";

        int days = (currentTime.Date - lastCommit.Date).Days;

        if (days == 0)
            return $"{lastCommit:HH:mm} today";

        if (days == 1)
            return $"{lastCommit:HH:mm} yesterday";

        if (days > 9 && days < 30)
            return $"{days}{FourSpacer}days ago";

        if (days < 30)
            return $"{days}{FiveSpacer}days ago";

        int months = (int)Math.Round(days / 30.0);

        if (months <= 1)
            return $"1{FiveSpacer}month ago";

        if (months <= 9)
            return $"{months}{FiveSpacer}months ago";

        return $"{months}{FourSpacer}months ago";
    }
}
