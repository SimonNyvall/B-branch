namespace Bbranch.CLI.Arguments;

using FlagSystem;
using FlagSystem.Flags;

public class Parse
{
    private static FlagCollection _flags = [];

    public static bool TryParseOptions(string[] args, out FlagCollection options)
    {
        _flags.Clear();

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

    private static FlagCollection MapOptionsToFlags(Dictionary<string, string?> options, int retry = 1)
    {
        foreach (KeyValuePair<string, string?> option in options)
        {
            IFlag? flag = option.Key switch
            {
                "--all" or "-a" => IFlag<AllFlag>.Create(option.Value),
                "--help" or "-h" => IFlag<HelpFlag>.Create(option.Value),
                "--contains" or "-c" => IFlag<ContainsFlag>.Create(option.Value),
                "--no-contains" or "-n" => IFlag<NoContainsFlag>.Create(option.Value),
                "--print-top" or "-p" => IFlag<PrintTopFlag>.Create(option.Value),
                "--quiet" or "-q" => IFlag<quietFlag>.Create(option.Value),
                "--remote" or "-r" => IFlag<RemoteFlag>.Create(option.Value),
                "--sort" or "-s" => IFlag<SortFlag>.Create(option.Value),
                "--track" or "-t" => IFlag<TrackFlag>.Create(option.Value),
                "--version" or "-v" => IFlag<VersionFlag>.Create(option.Value),
                "--pager" => IFlag<PagerFlag>.Create(option.Value),
                "--no-pager" => IFlag<NoPagerFlag>.Create(option.Value),
                _ => null
            };

            if (retry == 2 && flag is null)
            {
                throw new ArgumentException($"Invalid option: {option.Key}");
            }

            if (flag is null && retry == 1)
            {
                MapOptionsToFlags(SplitOptions(options, option), retry + 1);

                if (_flags.Count != 0)
                {
                    return _flags;
                }
            }

            if (flag is not null)
            {
                _flags.Add(flag);
            }
        }

        return _flags;
    }

    private static Dictionary<string, string?> SplitOptions(Dictionary<string, string?> options, KeyValuePair<string, string?> failOption)
    {
        if (!failOption.Key.StartsWith('-'))
        {
            throw new ArgumentException($"Consecutive option: {failOption.Key}");
        }

        if (failOption.Key.Length == 1)
        {
            throw new ArgumentException($"Invalid option: {failOption.Key}");
        }

        if (failOption.Key.Length > 2)
        {
            if (failOption.Key[1] == '-')
            {
                throw new ArgumentException($"Only one dash is allowed: {failOption.Key}");
            }
        }        

        foreach (char flag in failOption.Key.TrimStart('-'))
        {
            string key = $"-{flag}";

            if (options.ContainsKey(key))
            {
                throw new ArgumentException($"Duplicate option: {key}");
            }

            options[key] = string.Empty;
        }

        options.Remove(failOption.Key);

        return options;
    }
}