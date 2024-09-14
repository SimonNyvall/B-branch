namespace Bbranch.CLI.Arguments;

using FlagSystem;
using FlagSystem.Flags;

public class Parse
{
    public static bool TryParseOptions(string[] args, out FlagCollection options)
    {
        try
        {
            options = ParseArguments(args);

            return true;
        }
        catch (ArgumentException e)
        {
            Console.WriteLine(e.Message);

            options = [];

            return false;
        }
    }

    private static FlagCollection ParseArguments(string[] arguments)
    {
        Dictionary<string, string?> options = PopulateInput(arguments);

        return MapOptionsToFlags(options);
    }

    private static Dictionary<string, string?> PopulateInput(string[] arguments)
    {
        Dictionary<string, string?> options = [];

        for (int i = 0; i < arguments.Length; i++)
        {
            if (!DoesFlagStartWithDash(arguments[i]))
            {
                continue;
            }

            string option = arguments[i];

            if (IsNextValueAnOption(arguments, i))
            {
                if (IsOptionDuplicated(option, options!))
                {
                    throw new ArgumentException($"Duplicate option: {option}");
                }

                options[option] = arguments[++i];
                continue;
            }

            if (IsOptionDuplicated(option, options!))
            {
                throw new ArgumentException($"Duplicate option: {option}");
            }

            options[option] = null;
        }

        return options;
    }

    private static bool DoesFlagStartWithDash(string options) =>
        options.StartsWith("--") || options.StartsWith("-");

    private static bool IsNextValueAnOption(string[] arguments, int index) =>
        (index + 1) < arguments.Length && !(arguments[index + 1].StartsWith("--") || arguments[index + 1].StartsWith("-"));

    private static bool IsOptionDuplicated(string option, Dictionary<string, string> options) =>
        options.ContainsKey(option);

    private static FlagCollection MapOptionsToFlags(Dictionary<string, string?> options)
    {
        FlagCollection flags = [];

        foreach (KeyValuePair<string, string?> option in options)
        {
            IFlag flag = option.Key switch
            {
                "--all" or "-a" => IFlag<AllFlag>.Create(null),
                "--help" or "-h" => IFlag<HelpFlag>.Create(null),
                "--contains" or "-c" => IFlag<ContainsFlag>.Create(option.Value),
                "--no-contains" or "-n" => IFlag<NoContainsFlag>.Create(option.Value),
                "--print-top" or "-p" => IFlag<PrintTopFlag>.Create(option.Value),
                "--quite" or "-q" => IFlag<QuiteFlag>.Create(null),
                "--remote" or "-r" => IFlag<RemoteFlag>.Create(null),
                "--sort" or "-s" => IFlag<SortFlag>.Create(option.Value),
                "--track" or "-t" => IFlag<TrackFlag>.Create(null),
                "--version" or "-v" => IFlag<VersionFlag>.Create(null),
                _ => throw new ArgumentException($"Unknown option: {option.Key}")
            };

            flags.Add(flag);
        }

        return flags;
    }
}