using System.Reflection;

namespace Bbranch.GitService.OptionStrategies;

public static class MethodTimeLogger
{
    public static void Log(MethodBase methodBase, TimeSpan timeSpan, string message)
    {
        Console.WriteLine($"{methodBase.DeclaringType!.Name}.{methodBase.Name} {timeSpan}");
    }
}