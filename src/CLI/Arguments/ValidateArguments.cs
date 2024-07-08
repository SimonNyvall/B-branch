namespace CLI.ValidateArguments;

using Flags;

public class Validate
{
    public static void Arguments(Dictionary<FlagType, string> options)
    {
        var validators = new Action<Dictionary<FlagType, string>>[]
        {
            ValidateVersion,
            ValidateContains,
            ValidateAllRemote,
            ValidateSortValue,
            ValidatePrintTopValue,
        };

        foreach (var validator in validators)
        {
            validator(options);
        }
    }

    private static void ValidateVersion(Dictionary<FlagType, string> options)
    {
        if (options.ContainsKey(FlagType.Version) && options.Count > 1)
        {
            throw new ArgumentException("You cannot use --version with any other option");
        }
    }

    private static void ValidateContains(Dictionary<FlagType, string> options)
    {
        bool contains = (
            options.ContainsKey(FlagType.Contains) && options.ContainsKey(FlagType.Nocontains)
        );

        if (contains)
        {
            throw new ArgumentException("You cannot use both --contains and --no-contains");
        }
    }

    private static void ValidateAllRemote(Dictionary<FlagType, string> options)
    {
        if (options.ContainsKey(FlagType.All) && options.ContainsKey(FlagType.Remote))
        {
            throw new ArgumentException("You cannot use both --all and --remote");
        }
    }

    private static void ValidateSortValue(Dictionary<FlagType, string> options)
    {
        if (options.TryGetValue(FlagType.Sort, out string? value))
        {
            if (value == "date" || value == "name" || value == "ahead" || value == "behind")
            {
                return;
            }

            throw new ArgumentException(
                "Value for --sort is missing. Valid values are: date, name, ahead, behind"
            );
        }
    }

    private static void ValidatePrintTopValue(Dictionary<FlagType, string> options)
    {
        if (options.TryGetValue(FlagType.Printtop, out string? value))
        {
            if (!int.TryParse(value, out int numberValue)) throw new ArgumentException("Invalid value for --print-top");

            if (numberValue < 1) throw new ArgumentException("Value for --print-top must be greater than 0");
        }
    }
}
