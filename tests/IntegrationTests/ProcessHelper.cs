using System.Diagnostics;

namespace Bbranch.IntegrationTests;

internal class ProcessHelper
{
    public static Process GetDotnetProcess(params string[] flags)
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
}