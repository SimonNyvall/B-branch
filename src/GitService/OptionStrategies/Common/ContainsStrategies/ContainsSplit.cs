using Bbranch.Shared.TableData;

namespace Bbranch.GitService.OptionStrategies.Common.ContainsStrategies;

internal class ContainsSplit
{
    public static string[] SplitArgument(string argument)
    {
        return argument.Split(';');
    }
}