namespace Git.Options;

// This should not implement IOption and should be moved to a different place
public class VersionOptions
{
    public static void Execute()
    {
            Console.WriteLine("v1.1.0");
            Environment.Exit(0);
    }
}
