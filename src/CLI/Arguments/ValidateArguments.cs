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
    public static Result Arguments(Dictionary<FlagType, string> options)
    {
        foreach (var option in options)
        {
            System.Console.WriteLine(option.Key);
        }

        var validators = new Func<Dictionary<FlagType, string>, Result>[]
        {
            ValidateVersion,
            ValidateContains,
            ValidateAllRemote,
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

    private static Result ValidateVersion(Dictionary<FlagType, string> options)
    {
        if (options.ContainsKey(FlagType.Version) && options.Count > 1)
        {
            Error.Register("You cannot use --version with any other option");
            return Result.Error;
        }

        return Result.Success;
    }

    private static Result ValidateContains(Dictionary<FlagType, string> options)
    {
        bool contains = (
            options.ContainsKey(FlagType.Contains) && options.ContainsKey(FlagType.NoContains)
        );

        if (contains)
        {
            Error.Register("You cannot use both --contains and --no-contains");
            return Result.Error;
        }

        return Result.Success;
    }

    private static Result ValidateAllRemote(Dictionary<FlagType, string> options)
    {
        if (options.ContainsKey(FlagType.All) && options.ContainsKey(FlagType.Remote))
        {
            Error.Register("You cannot use both --all and --remote");
            return Result.Error;
        }

        return Result.Success;
    }

    private static Result ValidateSortValue(Dictionary<FlagType, string> options)
    {
        if (options.TryGetValue(FlagType.Sort, out string? value))
        {
            if (
                value == "date"
                || value == "name"
                || value == "ahead"
                || value == "behind"
            )
            {
                return Result.Success;
            }

            Error.Register(
                "Value for --sort is missing. Valid values are: date, name, ahead, behind"
            );
            return Result.Error;
        }

        return Result.Success;
    }
}
