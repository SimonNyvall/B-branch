namespace Bbranch.Branch.Info;

using System.Diagnostics;
using System.Text.RegularExpressions;

public class BranchInfo
{
    public string? GitPath { get; private set; } = null;

    public BranchInfo()
    {
        TrySetGitPath();
    }

    public (int, int) GetAheadBehind(string gitPath, string branchName)
    {
        int ahead = 0;
        int behind = 0;

        string checkLocalBranchCommand = $"rev-parse --verify {branchName}";
        string checkRemoteBranchCommand = $"rev-parse --verify origin/{branchName}";

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

        if (process is null) return (ahead, behind);

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

    public string? TryGetWorkingBranch(string gitPath)
    {
        try
        {
            return GetWorkingBranch(gitPath);
        }
        catch
        {
            return null;
        }
    }

    private string? GetWorkingBranch(string gitPath)
    {
        var HEADFile = File.ReadAllText(Path.Combine(gitPath, "HEAD")).Trim();

        if (HEADFile.StartsWith("ref:"))
        {
            var branchNameComponents = HEADFile.Split('/');

            var branchName = string.Join("/", branchNameComponents.Skip(2));

            return branchName;
        }

        return null;
    }

    public Dictionary<string, DateTime> GetNamesAndLastWirte(string gitPath)
    {
        var branches = new Dictionary<string, DateTime>();
        var branchDir = Path.Combine(gitPath, "refs", "heads");

        if (!Directory.Exists(branchDir)) throw new Exception("Branch directory does not exist");

        foreach (var file in Directory.GetFiles(branchDir, "*", SearchOption.AllDirectories))
        {
            var relativePath = Path.GetRelativePath(branchDir, file);
            var branchName = relativePath.Replace(Path.DirectorySeparatorChar, '/');
            branches.Add(branchName, File.GetLastWriteTime(file));
        }

        return branches
            .OrderByDescending(x => x.Value)
            .ToDictionary(x => x.Key, x => x.Value);
    }

    public string GetBranchDescription(string gitPath, string branchName)
    {
        if (!File.Exists(Path.Combine(gitPath, "EDIT_DESCRIPTION"))) return String.Empty;

        var descriptionFile = File.ReadAllText(Path.Combine(gitPath, "EDIT_DESCRIPTION"));

        var branches = GetNamesAndLastWirte(gitPath);

        if (!descriptionFile.Contains(branchName)) return String.Empty;

        var lines = descriptionFile.Split('\n');

        var linesWithoutComments = lines.Where(x => !x.StartsWith("#"));

        var description = string.Join(" ", linesWithoutComments);

        return description;
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

    private void TrySetGitPath()
    {
        const string command = "git";
        const string argument = "rev-parse --git-dir";

        ProcessStartInfo startInfo = new ProcessStartInfo(command, argument)
        {
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardError = true
        };

        using Process? process = Process.Start(startInfo);

        if (process == null) return;

        using StreamReader reader = process!.StandardOutput;

        string gitPath = reader.ReadToEnd().Trim();

        if (string.IsNullOrEmpty(gitPath)) return;

        GitPath = gitPath;
    }
}
