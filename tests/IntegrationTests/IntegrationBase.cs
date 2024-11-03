using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Bbranch.IntegrationTests;

public abstract partial class IntegrationBase
{
    protected static Process GetDotnetProcess(params string[] flags)
    {
        string combinedFlags = string.Join(" ", flags);

        string repoPath = Path.Combine(Directory.GetCurrentDirectory(), "../../../../../");
        Process process = new()
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"run --project ./src/CLI/CLI.csproj -- --no-pager {combinedFlags}",
                WorkingDirectory = repoPath,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        return process;
    }

    protected static Process GetDotnetProcess(bool allowPager, params string[] flags)
    {
        string combinedFlags = string.Join(" ", flags);

        string repoPath = Path.Combine(Directory.GetCurrentDirectory(), "../../../../../");
        Process process = new()
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"run --project ./src/CLI/CLI.csproj -- {combinedFlags}",
                WorkingDirectory = repoPath,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        return process;
    }

    protected async Task<(string output, string error)> RunProcessWithTimeoutAsync(Process process, int timeoutMilliseconds = 20000)
    {
        process.Start();

        var completedTask = await Task.WhenAny(
            Task.Run(() => process.WaitForExit()),
            Task.Delay(timeoutMilliseconds)
        );

        if (!completedTask.IsCompleted)
        {
            process.Kill();
            throw new Exception("Process timed out");
        }

        string output = await process.StandardOutput.ReadToEndAsync();
        string error = await process.StandardError.ReadToEndAsync();

        process.WaitForExit();

        return (output, error);
    }

    protected static void AssertHeader(string[] headerLines)
    {
        Assert.True(headerLines.Length >= 2, $"Header lines does not contain enought lines for header print. Expected at least 2 lines. Actual: {headerLines.Length} Lines: {string.Join('\n', headerLines)}");

        Assert.Contains("Ahead", headerLines[0]);
        Assert.Contains("Behind", headerLines[0]);
        Assert.Contains("Branch name", headerLines[0]);
        Assert.Contains("Last commit", headerLines[0]);

        Assert.True(headerLines[1].All(c => c == '|' || c == '-' || c == ' ' || c == '\r'));
    }

    protected static (int ahead, int behind) GetAheadBehindFromString(string line)
    {
        int ahead = 0;
        int behind = 0;

        Match match = aheadBehindPattern().Match(line);

        if (!match.Success) throw new Exception($"Failed to parse ahead/behind... Line: {line}");

        ahead = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
        behind = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);

        return (ahead, behind);
    }

    [GeneratedRegex(@"\s*(\d+)\s*\|\s*(\d+)", RegexOptions.Compiled)]
    private static partial Regex aheadBehindPattern();
}