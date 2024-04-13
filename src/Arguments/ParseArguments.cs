using Bbranch.ErrorHandler;

namespace Bbranch.ParseArguments;

public class Parse
{
    public static Dictionary<string, string> Arguments(string[] arguments)
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
}
