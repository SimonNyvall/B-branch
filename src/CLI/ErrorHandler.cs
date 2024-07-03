namespace CLI.ErrorHandler;

public class Error
{
    private static List<string> Errors { get; } = [];

    public static void Register(string message) => Errors.Add(message);

    public static void Log()
    {
        if (Errors.Count == 0)
            return;

        Console.ForegroundColor = ConsoleColor.Red;

        foreach (var error in Errors)
        {
            Console.WriteLine($"* {error}");
        }

        Console.ResetColor();
    }
}
