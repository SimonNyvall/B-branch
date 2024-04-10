namespace Git.Options
{
    public class VersionOptions
    {
        public static void Handle(Dictionary<string, string> options)
        {
            if (options.ContainsKey("version") || options.ContainsKey("v"))
            {
                Console.WriteLine("v1.1.0");
                Environment.Exit(0);
            }
        }
    }
}
