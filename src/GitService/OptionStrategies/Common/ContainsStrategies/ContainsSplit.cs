namespace Bbranch.GitService.OptionStrategies.Common.ContainsStrategies;

internal sealed class ContainsSplit
{
    public static string[] SplitArgument(string argument)
    {
        return argument.Split(';');
    }
}