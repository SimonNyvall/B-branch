namespace Bbranch.ParseArguments;

public class Parse
{
    public static Dictionary<string, string> Arguments(string[] arguments)
    {
        Dictionary<string, string> options = [];

        for (int i = 0; i < arguments.Length; i++)
        {
            if (!(arguments[i].StartsWith("--") || arguments[i].StartsWith('-')))
                continue;

            string option = arguments[i].TrimStart('-');

            if ((i + 1) < arguments.Length && !arguments[i + 1].StartsWith("--"))
            {
                options[option] = arguments[++i];
                continue;
            }

            options[option] = "true";
        }

        return options;
    }
}
