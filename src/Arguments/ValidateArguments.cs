using Bbranch.ErrorHandler;

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
        ("--all", "-a"),
        ("--help", "-h"),
        ("--quiet", "-q"),
        ("--sort", "-s"),
        ("--contains", "-c"),
        ("--no-contains", "-C"),
        ("--remote", "-r"),
        ("--track", "-t"),
        ("--version", "-v")
    };

    public static Result Arguments(Dictionary<string, string> options)
    {
        var validators = new Func<Dictionary<string, string>, Result>[]
        {
            ValidateCount,
            ValidateShortArgument,
            ValidateLongArgument,
            ValidateVersion,
            ValidateContains,
            ValidateAllRemote
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

    private static Result ValidateCount(Dictionary<string, string> options)
    {
        if (options.Count == 0)
            return Result.Error;

        return Result.Success;
    }

    private static Result ValidateShortArgument(Dictionary<string, string> options)
    {
        if (options.Keys.Any(ops => !ValidArgs.Any(arg => arg.Item2 == ops)))
        {
            Error.Register("Invalid short option specified");
            return Result.Error;
        }

        return Result.Success;
    }

    private static Result ValidateLongArgument(Dictionary<string, string> options)
    {
        if (options.Keys.Any(ops => !ValidArgs.Any(arg => arg.Item1 == ops)))
        {
            Error.Register("Invalid long option specified");
            return Result.Error;
        }

        return Result.Success;
    }

    private static Result ValidateVersion(Dictionary<string, string> options)
    {
        if (options.ContainsKey("version") && options.Count > 1)
        {
            Error.Register("You cannot use --version with any other option");
            return Result.Error;
        }

        return Result.Success;
    }

    private static Result ValidateContains(Dictionary<string, string> options)
    {
        if (options.ContainsKey("contains") && options.ContainsKey("no-contains"))
        {
            Error.Register("You cannot use both --contains and --no-contains");
            return Result.Error;
        }

        return Result.Success;
    }

    private static Result ValidateAllRemote(Dictionary<string, string> options)
    {
        if (options.ContainsKey("all") && options.ContainsKey("remote"))
        {
            Error.Register("You cannot use both --all and --remote");
            return Result.Error;
        }

        return Result.Success;
    }

    private static Result ValidateTrackFlag(Dictionary<string, string> options)
    {
        if (!options.ContainsKey("track") || !options.ContainsKey("t"))
            return Result.Success;

        if (options.ContainsKey("quite") || options.ContainsKey("q"))
        {
            Error.Register("You cannot use --track with --quiet");
            return Result.Error;
        }

        return Result.Success;
    }
}
