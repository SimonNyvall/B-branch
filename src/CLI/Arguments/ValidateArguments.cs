namespace CLI.ValidateArguments;

using Flags;

public class Validate
{
    public static bool ValidateOptions(IFlagCollection options)
    {
        try
        {
            ValidateVersion(options);
            ValidateContains(options);
            ValidateAllRemote(options);
            ValidateSortValue(options);
            ValidatePrintTopValue(options);

            return true;
        }
        catch (ArgumentException e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }

    private static void ValidateVersion(IFlagCollection options)
    {
        if (options.Contains<VersionFlag>() && options.Count > 1)
        {
            throw new ArgumentException("You cannot use --version with any other option");
        }
    }

    private static void ValidateContains(IFlagCollection options)
    {
        if (options.Contains<ContainsFlag>() && options.Contains<NoContainsFlag>())
        {
            throw new ArgumentException("You cannot use both --contains and --no-contains");
        }
    }

    private static void ValidateAllRemote(IFlagCollection options)
    {
        if (options.Contains<AllFlag>() && options.Contains<RemoteFlag>())
        {
            throw new ArgumentException("You cannot use both --all and --remote");
        }
    }

    private static void ValidateSortValue(IFlagCollection options)
    {
        if (options.Contains<SortFlag>(out var sortFlag))
        {
            if (sortFlag.Value == "date" || sortFlag.Value == "name" || sortFlag.Value == "ahead" || sortFlag.Value == "behind")
            {
                return;
            }

            throw new ArgumentException(
                "Value for --sort is missing. Valid values are: date, name, ahead, behind"
            );
        }
    }

    private static void ValidatePrintTopValue(IFlagCollection options)
    {
        if (options.Contains<PrintTopFlag>(out var printTopFlag))
        {
            if (!int.TryParse(printTopFlag.Value, out int numberValue))
            {
                throw new ArgumentException("Invalid value for --print-top");
            }

            if (numberValue < 1)
            {
                throw new ArgumentException("Value for --print-top must be greater than 0");
            }
        }
    }
}
