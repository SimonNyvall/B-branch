using CLI.Flags;

namespace CLI.ParseArguments;

public class Parse
{
    private static readonly string[] flags = [
        "--help", "-h",
        "--track", "-t",
        "--sort", "-s",
        "--contains", "-c",
        "--no-contains", "-n",
        "--all", "-a",
        "--remote", "-r",       
        "--quite", "-q",
        "--print-top", "-p",
        "--version", "-v",
    ];

    public static bool TryParseOptions(string[] args, out Dictionary<FlagType, string> options)
    {
        try
        {
            options = Parse.Arguments(args);

            return true;
        }
        catch (ArgumentException e)
        {
            Console.WriteLine(e.Message);
            Environment.Exit(1);

            options = [];

            return false;
        }
    }

    private static Dictionary<FlagType, string> Arguments(string[] arguments)
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

            if (!flags.Contains(arguments[i].ToLower()))
            {
                throw new ArgumentException($"Invalid option: {arguments[i]}");
            }

            string option = arguments[i].Replace("-", string.Empty);

            if (
                (i + 1) < arguments.Length
                && !(arguments[i + 1].StartsWith("--") || arguments[i + 1].StartsWith("-"))
            )
            {
                if (IsOptionDuplicated(option, options))
                {
                    throw new ArgumentException($"Duplicate option: {option}"); 
                }

                options[option] = arguments[++i];
                continue;
            }

            if (IsOptionDuplicated(option, options))
            {
                throw new ArgumentException($"Duplicate option: {option}");                
            }

            options[option] = string.Empty;
        }

        return options;
    }

    private static bool IsOptionDuplicated(string option, Dictionary<string, string> options)
    {
        if (options.ContainsKey(option)) throw new ArgumentException($"Duplicate option: {option}");
                
        return false;
    }

    private static Dictionary<FlagType, string> MapOptionsToFlags(Dictionary<string, string> options)
    {
        Dictionary<FlagType, string> flags = [];

        foreach (KeyValuePair<string, string> option in options)
        {
            if (Enum.TryParse(ToTitle(option.Key), out FlagType flag))
            {
                flags[flag] = option.Value;
            }
            else if (Enum.TryParse(option.Key, out ShortFlagType shortFlag))
            {
                flags[GetLongFlag(shortFlag)] = option.Value;
            }
            else
            {
                throw new ArgumentException($"Invalid option: {option.Key}");
            }
        }

        return flags;
    }

    private static string ToTitle(string input)
    {
        return input.Substring(0, 1).ToUpper() + input.Substring(1);
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
            ShortFlagType.nc => FlagType.Nocontains,
            ShortFlagType.r => FlagType.Remote,
            ShortFlagType.t => FlagType.Track,
            ShortFlagType.v => FlagType.Version,
            ShortFlagType.p => FlagType.Printtop,
            _ => FlagType.All
        };
    }
}
