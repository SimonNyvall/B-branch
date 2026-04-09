namespace Bbranch.CLI.Arguments;

using FlagSystem;
using FlagSystem.Flags;

public sealed class Validate
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
            ValidatePrintTopWithNull(options);
            ValidateTrackWithNull(options);
            ValidatePagerWithNoPager(options);
            ValidateNonValueFlags(options);

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
            throw new ArgumentException("fatal: --version cannot be used with any other option");
        }
    }

    private static void ValidateContains(FlagCollection options)
    {
        if (options.Contains<ContainsFlag>() && options.Contains<NoContainsFlag>())
        {
            throw new ArgumentException("fatal: Cannot use both --contains and --no-contains");
        }
    }

    private static void ValidateAllRemote(FlagCollection options)
    {
        if (options.Contains<AllFlag>() && options.Contains<RemoteFlag>())
        {
            throw new ArgumentException("fatal: Cannot use both --all and --remote");
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

            throw new ArgumentException("fatal: '--sort' must a criterion of 'date', 'name', 'ahead', or 'behind'");
        }
    }

    private static void ValidatePrintTopValue(FlagCollection options)
    {
        if (options.Contains<PrintTopFlag>(out var printTopFlag))
        {
            if (!int.TryParse(printTopFlag.Value.ToString(), out int numberValue) && printTopFlag.Value.ToString() != string.Empty)
            {
                throw new ArgumentException("fatal: Value for --print-top must be an integer");
            }

            if (numberValue < 1 && printTopFlag.Value.ToString() != string.Empty)
            {
                throw new ArgumentException("fatal: Value for --print-top must be greater than 0");
            }
        }
    }

    private static void ValidateContainsWithNull(FlagCollection options)
    {
        if (!options.Contains<ContainsFlag>(out var containsFlag)) return;

        if (containsFlag.Value is null || containsFlag.Value.ToString() == string.Empty) throw new ArgumentException("fatal: Value for --contains is missing");
    }

    private static void ValidateNoContainsWithNull(FlagCollection options)
    {
        if (!options.Contains<NoContainsFlag>(out var noContainsFlag)) return;

        if (noContainsFlag.Value is null || noContainsFlag.Value.ToString() == string.Empty) throw new ArgumentException("fatal: Value for --no-contains is missing");
    }

    private static void ValidatePrintTopWithNull(FlagCollection options)
    {
        if (!options.Contains<PrintTopFlag>(out var printTopFlag)) return;

        if (printTopFlag.Value is null || printTopFlag.Value.ToString() == string.Empty) throw new ArgumentException("fatal: Value for --print-top is missing");
    }

    private static void ValidateTrackWithNull(FlagCollection options)
    {
        if (!options.Contains<TrackFlag>(out var trackFlag)) return;

        if (trackFlag.Value is null || trackFlag.Value.ToString() == string.Empty) throw new ArgumentException("fatal: Value for --track is missing");
    }

    private static void ValidatePagerWithNoPager(FlagCollection options)
    {
        if (!(options.Contains<PagerFlag>() || options.Contains<NoPagerFlag>())) return;

        if (options.Contains<PagerFlag>() && options.Contains<NoPagerFlag>())
        {
            throw new ArgumentException("fatal: Cannot use both --pager and --no-pager");
        }
    }

    private static void ValidateNonValueFlags(FlagCollection options)
    {
        if (options.Contains<HelpFlag>(out var helpFlag))
        {
            if (helpFlag.Value.ToString() != string.Empty)
            {
                throw new ArgumentException("fatal: Value for --help is not allowed");
            }
        }

        if (options.Contains<VersionFlag>(out var versionFlag))
        {
            if (versionFlag.Value.ToString() != string.Empty)
            {
                throw new ArgumentException("fatal: Value for --version is not allowed");
            }
        }

        if (options.Contains<AllFlag>(out var allFlag))
        {
            if (allFlag.Value.ToString() != string.Empty)
            {
                throw new ArgumentException("fatal: Value for --all is not allowed");
            }
        }

        if (options.Contains<RemoteFlag>(out var remoteFlag))
        {
            if (remoteFlag.Value.ToString() != string.Empty)
            {
                throw new ArgumentException("fatal: Value for --remote is not allowed");
            }
        }

        if (options.Contains<quietFlag>(out var quietFlag))
        {
            if (quietFlag.Value.ToString() != string.Empty)
            {
                throw new ArgumentException("fatal: Value for --quiet is not allowed");
            }
        }

        if (options.Contains<PagerFlag>(out var pagerFlag))
        {
            if (pagerFlag.Value.ToString() != string.Empty)
            {
                throw new ArgumentException("fatal: Value for --pager is not allowed");
            }
        }

        if (options.Contains<NoPagerFlag>(out var noPagerFlag))
        {
            if (noPagerFlag.Value.ToString() != string.Empty)
            {
                throw new ArgumentException("fatal: Value for --no-pager is not allowed");
            }
        }
    }
}
