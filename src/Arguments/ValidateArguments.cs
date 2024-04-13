using Bbranch.ErrorHandler;
using Bbranch.Flags;

namespace Bbranch.ValidateArguments;

public enum Result
{
    Success,
    Error
}

public class Validate
{
    private static readonly (string, string)[] ValidArgs =
    {
        (Flag.All, Flag.A),
        (Flag.Help, Flag.H),
        (Flag.Quiet, Flag.Q),
        (Flag.Sort, Flag.S),
        (Flag.Contains, Flag.C),
        (Flag.NoContains, Flag.NoC),
        (Flag.Remote, Flag.R),
        (Flag.Track, Flag.T),
        (Flag.Version, Flag.V)
    };

    public static Result Arguments(Dictionary<string, string> options)
    {
        var validators = new Func<Dictionary<string, string>, Result>[]
        {
            ValidateValidArgument,
            ValidateVersion,
            ValidateContains,
            ValidateAllRemote,
            ValidateTrackQuiteFlags,
            ValidateTrackValue,
            ValidateSortValue,
        };

        foreach (var validator in validators)
        {
            Result result = validator(options);

            if (result == Result.Error)
            {
                Error.Log();
                return Result.Error;
            }
        }

        return Result.Success;
    }

    private static Result ValidateValidArgument(Dictionary<string, string> options)
    {
        foreach (var option in options)
        {
            bool isValid = false;
            foreach (var validArg in ValidArgs)
            {
                if (option.Key == validArg.Item1 || option.Key == validArg.Item2)
                {
                    isValid = true;
                    break;
                }
            }
            if (!isValid)
            {
                Error.Register($"Invalid option: {option.Key}");
                return Result.Error;
            }
        }

        return Result.Success;
    }

    private static Result ValidateVersion(Dictionary<string, string> options)
    {
        if (options.ContainsKey(Flag.Version) && options.Count > 1)
        {
            Error.Register("You cannot use --version with any other option");
            return Result.Error;
        }

        return Result.Success;
    }

    private static Result ValidateContains(Dictionary<string, string> options)
    {
        bool contains = (
            options.ContainsKey(Flag.Contains)
            || options.ContainsKey(Flag.C) && options.ContainsKey(Flag.NoContains)
            || options.ContainsKey(Flag.NoC)
        );

        if (contains)
        {
            Error.Register("You cannot use both --contains and --no-contains");
            return Result.Error;
        }

        return Result.Success;
    }

    private static Result ValidateAllRemote(Dictionary<string, string> options)
    {
        if (options.ContainsKey(Flag.All) && options.ContainsKey(Flag.Remote))
        {
            Error.Register("You cannot use both --all and --remote");
            return Result.Error;
        }

        return Result.Success;
    }

    private static Result ValidateTrackQuiteFlags(Dictionary<string, string> options)
    {
        bool quiet = (
            (options.ContainsKey(Flag.Quiet) || options.ContainsKey(Flag.Q))
            && (options.ContainsKey(Flag.Track) || options.ContainsKey(Flag.T))
        );

        if (quiet)
        {
            Error.Register("You cannot use both --quiet and --track");
            return Result.Error;
        }

        return Result.Success;
    }

    private static Result ValidateTrackValue(Dictionary<string, string> options)
    {
        if (options.ContainsKey(Flag.Track))
        {
            if (options[Flag.Track] != string.Empty)
                return Result.Success;

            Error.Register("You must provide a value for --track");
            return Result.Error;
        }

        if (options.ContainsKey(Flag.T))
        {
            if (options[Flag.T] != string.Empty)
                return Result.Success;

            Error.Register("You must provide a value for -t");
            return Result.Error;
        }

        return Result.Success;
    }

    private static Result ValidateSortValue(Dictionary<string, string> options)
    {
        if (options.ContainsKey(Flag.Sort))
        {
            if (
                options[Flag.Sort] == "date"
                || options[Flag.Sort] == "name"
                || options[Flag.Sort] == "ahead"
                || options[Flag.Sort] == "behind"
            )
            {
                return Result.Success;
            }

            Error.Register(
                "Value for --sort is missing. Valid values are: date, name, ahead, behind"
            );
            return Result.Error;
        }

        if (options.ContainsKey(Flag.S))
        {
            if (
                options[Flag.S] == "date"
                || options[Flag.S] == "name"
                || options[Flag.S] == "ahead"
                || options[Flag.S] == "behind"
            )
            {
                return Result.Success;
            }

            Error.Register("Value for -s is missing. Valid values are: date, name, ahead, behind");
            return Result.Error;
        }

        return Result.Success;
    }
}
