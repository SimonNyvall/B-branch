namespace Bbranch.CLI.Options;

public static class VersionOption
{
    public static void Execute()
    {
        Console.WriteLine("v1.3.0");
        Environment.Exit(0);
    }
}
