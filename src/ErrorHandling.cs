namespace Bbranch.ErrorHandling;

public class Error
{
    public static void HandleGitDirNotFound(string? gitPath)
    {
        if (gitPath is not null) return;

        PrintError("Git directory not found.");
        Environment.Exit(1);
    }

    public static void HandleNoWorkingBranch(string? workingBranch)
    {
        if (workingBranch is not null) return;

        PrintError("No working branch found.");
        Environment.Exit(1);
    }

    private static void PrintError(string message)
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine(message);
        Console.ResetColor();
    }
}
