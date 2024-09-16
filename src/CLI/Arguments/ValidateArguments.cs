namespace Bbranch.CLI.Arguments;

using FlagSystem;
using FlagSystem.Flags;

public class Validate
{
    public static bool ValidateOptions(FlagCollection options)
    {
        try
        {
            ValidateVersion(options);
            ValidateContains(options);
            ValidateAllRemote(options);
            ValidateSortValue(options);
            ValidatePrintTopValue(options);
            ValidateContainsWithNull(options);
            ValidateNoContainsWithNull(options);
            ValidateSortWithNull(options);
            ValidatePrintTopWithNull(options);
            ValidateTrackWithNull(options);

            return true;
        }
        catch (ArgumentException e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }

    private static void ValidateVersion(FlagCollection options)
    {
        if (options.Contains<VersionFlag>() && options.Count > 1)
        {
            throw new ArgumentException("You cannot use --version with any other option");
        }
    }

    private static void ValidateContains(FlagCollection options)
    {
        if (options.Contains<ContainsFlag>() && options.Contains<NoContainsFlag>())
        {
            throw new ArgumentException("You cannot use both --contains and --no-contains");
        }
    }

    private static void ValidateAllRemote(FlagCollection options)
    {
        if (options.Contains<AllFlag>() && options.Contains<RemoteFlag>())
        {
            throw new ArgumentException("You cannot use both --all and --remote");
        }
    }

    private static void ValidateSortValue(FlagCollection options)
    {
        if (options.Contains<SortFlag>(out var sortFlag))
        {
            if (sortFlag.Value.ToString() == "date" || sortFlag.Value.ToString() == "name" || sortFlag.Value.ToString() == "ahead" || sortFlag.Value.ToString() == "behind")
            {
                return;
            }

            throw new ArgumentException(
                "Value for --sort is missing. Valid values are: date, name, ahead, behind"
            );
        }
    }

    private static void ValidatePrintTopValue(FlagCollection options)
    {
        if (options.Contains<PrintTopFlag>(out var printTopFlag))
        {
            if (!int.TryParse(printTopFlag.Value.ToString(), out int numberValue))
            {
                throw new ArgumentException("Invalid value for --print-top");
            }

            if (numberValue < 1)
            {
                throw new ArgumentException("Value for --print-top must be greater than 0");
            }
        }
    }

    private static void ValidateContainsWithNull(FlagCollection options)
    {
        if (!options.Contains<ContainsFlag>(out var containsFlag)) return;

        if (containsFlag.Value is null || containsFlag.Value.ToString() == string.Empty) throw new ArgumentException("Value for --contains is missing");
    }

    private static void ValidateNoContainsWithNull(FlagCollection options)
    {
        if (!options.Contains<NoContainsFlag>(out var noContainsFlag)) return;

        if (noContainsFlag.Value is null || noContainsFlag.Value.ToString() == string.Empty) throw new ArgumentException("Value for --no-contains must be null");
    }

    private static void ValidateSortWithNull(FlagCollection options)
    {
        if (!options.Contains<SortFlag>(out var sortFlag)) return;

        if (sortFlag.Value is null) throw new ArgumentException("Value for --sort is missing");
    }

    private static void ValidatePrintTopWithNull(FlagCollection options)
    {
        if (!options.Contains<PrintTopFlag>(out var printTopFlag)) return;

        if (printTopFlag.Value is null || printTopFlag.Value.ToString() == string.Empty) throw new ArgumentException("Value for --print-top is missing");
    }

    private static void ValidateTrackWithNull(FlagCollection options)
    {
        if (!options.Contains<TrackFlag>(out var trackFlag)) return;

        if (trackFlag.Value is null || trackFlag.Value.ToString() == string.Empty) throw new ArgumentException("Value for --track is missing");
    }
}
