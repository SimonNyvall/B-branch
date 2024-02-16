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

        const string command = "git";
        string arguments = $"rev-list --left-right --count {branchName}...origin/main";

        const string pattern = @"(\d+)\s+(\d+)";

        ProcessStartInfo startInfo = new ProcessStartInfo(command, arguments)
        {
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            WorkingDirectory = gitPath
        };

        using Process? process = Process.Start(startInfo);

        if (process == null) return (ahead, behind);

        using StreamReader reader = process!.StandardOutput;

        string result = reader.ReadToEnd();
        var parts = result.Split(' ');

        foreach (var part in parts)
        {
            if (Regex.IsMatch(part, pattern))
            {
                var match = Regex.Match(part, pattern);
                ahead = int.Parse(match.Groups[1].Value);
                behind = int.Parse(match.Groups[2].Value);
            }
        }

        return (ahead, behind);
    }

    public static Dictionary<string, DateTime> GetNamesAndLastWirte(string gitPath)
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
