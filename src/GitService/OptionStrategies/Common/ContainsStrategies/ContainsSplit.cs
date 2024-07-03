namespace Git.Options;

using Shared.TableData;

internal class ContainsSplit
{
    public static string[] SplitArgument(string argument)
    {
        return argument.Split(';');
    }
}