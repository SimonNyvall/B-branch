using Bbranch.Flags;
using Bbranch.ErrorHandler;

namespace Bbranch.ParseArguments;

// TODO fix bug where I count the number of "-"
public class Parse
{
    public static Dictionary<FlagType, string> Arguments(string[] arguments)
    {
        Dictionary<string, string> options = PopulateInput(arguments);

        return MapOptionsToFlags(options);
    }

    private static Dictionary<string, string> PopulateInput(string[] arguments)
    {
        Dictionary<string, string> options = [];

        for (int i = 0; i < arguments.Length; i++)
        {
            if (!(arguments[i].StartsWith("--") || arguments[i].StartsWith('-')))
            {
                continue;
            }

            string option = arguments[i].TrimStart('-');

            if (
                (i + 1) < arguments.Length
                && !(arguments[i + 1].StartsWith("--") || arguments[i + 1].StartsWith("-"))
            )
            {
                if (IsOptionDuplicated(option, options))
                {
                    Error.Log();
                    Environment.Exit(1);
                }

                options[option] = arguments[++i];
                continue;
            }

            if (IsOptionDuplicated(option, options))
            {
                Error.Log();
                Environment.Exit(1);
            }

            options[option] = string.Empty;
        }

        return options;
    }

    private static bool IsOptionDuplicated(string option, Dictionary<string, string> options)
    {
        if (options.ContainsKey(option))
        {
            Error.Register($"Duplicate option: {option}");
            return true;
        }

        return false;
    }

    private static Dictionary<FlagType, string> MapOptionsToFlags(Dictionary<string, string> options)
    {
        Dictionary<FlagType, string> flags = [];

        foreach (KeyValuePair<string, string> option in options)
        {
            if (Enum.TryParse(option.Key, out FlagType flag))
            {
                flags[flag] = option.Value;
            }
            else if (Enum.TryParse(option.Key, out ShortFlagType shortFlag))
            {
                flags[GetLongFlag(shortFlag)] = option.Value;
            }
            else
            {
                Error.Register($"Invalid option: {option.Key}");
                Environment.Exit(1);            
            }
        }

        return flags;
    }

    private static FlagType GetLongFlag(ShortFlagType shortFlag)
    {
        return shortFlag switch
        {
            ShortFlagType.a => FlagType.All,
            ShortFlagType.h => FlagType.Help,
            ShortFlagType.q => FlagType.Quiet,
            ShortFlagType.s => FlagType.Sort,
            ShortFlagType.c => FlagType.Contains,
            ShortFlagType.nc => FlagType.NoContains,
            ShortFlagType.r => FlagType.Remote,
            ShortFlagType.t => FlagType.Track,
            ShortFlagType.v => FlagType.Version,
            _ => FlagType.All
        };
    }
}
