namespace Bbranch.Branch.Info;

using System.Diagnostics;
using System.Text.RegularExpressions;

public class Branch
{
    public string? GitPath { get; private set; } = null;

    public Branch()
    {
        SetGitPath();
    }

    public (int, int) GetAheadBehind(string gitPath, string branchName)
    {
        int ahead = 0;
        int behind = 0;

        string checkLocalBranchCommand = $"git rev-parse --verify {branchName}";
        string checkRemoteBranchCommand = $"git rev-parse --verify origin/{branchName}";

        if (!ExecuteGitCommand(gitPath, checkLocalBranchCommand) || !ExecuteGitCommand(gitPath, checkRemoteBranchCommand))
        {
            return (ahead, behind);
        }

        const string command = "git";
        string arguments = $"rev-list --left-right --count {branchName}...origin/{branchName}";

        ProcessStartInfo startInfo = new ProcessStartInfo(command, arguments)
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            WorkingDirectory = gitPath
        };

        using Process? process = Process.Start(startInfo);

        if (process == null) return (ahead, behind);

        string result = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();
        process.WaitForExit();

        if (!string.IsNullOrEmpty(error))
        {
            Console.WriteLine($"Error: {error}");
            return (ahead, behind);
        }

        const string pattern = @"(\d+)\s+(\d+)";
        Match match = Regex.Match(result, pattern);
        if (match.Success)
        {
            ahead = int.Parse(match.Groups[1].Value);
            behind = int.Parse(match.Groups[2].Value);
        }

        return (ahead, behind);
    }

    private bool ExecuteGitCommand(string gitPath, string arguments)
    {
        ProcessStartInfo startInfo = new ProcessStartInfo("git", arguments)
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            WorkingDirectory = gitPath
        };

        using Process? process = Process.Start(startInfo);
        if (process == null) return false;

        string error = process.StandardError.ReadToEnd();
        process.WaitForExit();

        return string.IsNullOrEmpty(error) && process.ExitCode == 0;
    }

    public Dictionary<string, DateTime> GetNamesAndLastWirte(string gitPath)
    {
        var branches = new Dictionary<string, DateTime>();
        var branchDir = Path.Combine(gitPath, "refs", "heads");

        if (!Directory.Exists(branchDir)) throw new Exception("Branch directory does not exist");

        foreach (var file in Directory.GetFiles(branchDir))
        {
            branches.Add(Path.GetFileName(file), File.GetLastWriteTime(file));
        }

        return branches
            .OrderByDescending(x => x.Value)
            .ToDictionary(x => x.Key, x => x.Value);
    }

    private void SetGitPath()
    {
        const string command = "git";
        const string argument = "rev-parse --git-dir";

        ProcessStartInfo startInfo = new ProcessStartInfo(command, argument)
        {
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using Process? process = Process.Start(startInfo);

        if (process == null) return;

        using StreamReader reader = process!.StandardOutput;

        GitPath = reader.ReadToEnd().Trim();
    }
}
