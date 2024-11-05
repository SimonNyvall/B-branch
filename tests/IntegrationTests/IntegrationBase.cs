using System.Text;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Bbranch.IntegrationTests;

public abstract partial class IntegrationBase
{
    protected void WarmUp()
    {
        using var warpUpProcess = GetBbranchProcessWithoutPager();
        var (output, error) = RunProcessWithTimeoutAsync(warpUpProcess).GetAwaiter().GetResult();
    }

    protected static Process GetBbranchProcessWithoutPager(params string[] flags)
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
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        return process;
    }

    protected static Process GetBbranchProcess(params string[] flags)
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
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        return process;
    }

    protected async Task<(string output, string error)> RunProcessWithTimeoutAsync(Process process)
    {
       var outputBuilder = new StringBuilder();
        var errorBuilder = new StringBuilder();

        process.OutputDataReceived += (sender, args) =>
        {
            if (args.Data != null) outputBuilder.AppendLine(args.Data);
        };

        process.ErrorDataReceived += (sender, args) =>
        {
            if (args.Data != null) errorBuilder.AppendLine(args.Data);
        };

        const int timeoutMilliseconds = 120000;

        process.Start(); // Start the process
        process.BeginOutputReadLine(); // Begin asynchronous reading of standard output
        process.BeginErrorReadLine(); // Begin asynchronous reading of standard error

        using var cts = new CancellationTokenSource(timeoutMilliseconds);
        
        // Wait for either the process to exit or timeout
        var completedTask = await Task.WhenAny(
            Task.Run(() => process.WaitForExit()),
            Task.Delay(Timeout.Infinite, cts.Token) // This will complete when cancellation occurs
        );

        if (!completedTask.IsCompleted)
        {
            process.Kill(); // Process has timed out, kill it
            throw new Exception("Process timed out");
        }

        // After the process exits, retrieve the output and error
        string output = outputBuilder.ToString();
        string error = errorBuilder.ToString();

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