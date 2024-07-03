using System.Diagnostics;

namespace Git.Base;

public class Execute
{
    public static string CurrentWorkingDirectory { get; set; } = Directory.GetCurrentDirectory();

    private static Execute? _instance;

    private Execute() { }

    public static Execute GetInstance()
    {
        if (_instance is not null)
            return _instance;

        lock (typeof(Execute))
        {
            _instance = _instance is null ? new Execute() : _instance;
        }

        return _instance;
    }

    public string ExecuteCommand(string command)
    {
        using Process process = GetDefaultGitProcess(command);

        process.Start();

        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();

        process.WaitForExit();

        return output + error;
    }

    private static Process GetDefaultGitProcess(string arguments)
    {
        Process process = new();

        process.StartInfo.FileName = "git";
        process.StartInfo.Arguments = arguments;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.CreateNoWindow = true;

        if (!Directory.Exists(CurrentWorkingDirectory)) throw new DirectoryNotFoundException();

        process.StartInfo.WorkingDirectory = CurrentWorkingDirectory;

        return process;
    }
}
