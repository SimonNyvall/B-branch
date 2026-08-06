using System.Reflection;

/// <summary>
/// This will not be a part of the final assembly and is noly used for measuring the compute times.
/// Use the command 'dotnet build -p:timer=true' to include this in the final build.
/// And then 'dotnet run --no-build' to run the project with the prior build options.
/// </summary>
public static class MethodTimeLogger
{
    public static void Log(MethodBase methodBase, long milliseconds, string message)
    {
        var logMessage = $"{methodBase.DeclaringType}.{methodBase.Name} took {milliseconds}";

        if (!string.IsNullOrEmpty(message))
        {
            logMessage += $" with param {message}";
        }

        Console.WriteLine(logMessage);
    }
}
