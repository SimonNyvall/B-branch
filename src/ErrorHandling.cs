namespace Bbranch.Branch.ErrorHandling;

public class ErrorHandling
{
    public static void HandleGitDirNotFound()
    {
        PrintError("Git directory not found.");
        Environment.Exit(1);
    }

    public static void HandleNoWorkingBranch()
    {
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
