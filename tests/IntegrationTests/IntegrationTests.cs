using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Globalization;

namespace IntegrationTests;

public partial class IntegrationTest
{
    private Process GetDotnetProcess()
    {
        string repoPath = Path.Combine(Directory.GetCurrentDirectory(), "../../../../../");
        Process process = new() 
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = "run --project ./src/CLI/CLI.csproj",
                WorkingDirectory = repoPath,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        return process;
    }
    
    [Fact]
    public void IntegrationTest_ValidOutput()
    {
        using var process = GetDotnetProcess();
        process.Start();

        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();

        process.WaitForExit();

        Assert.True(string.IsNullOrEmpty(error));

        string[] lines = output.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
    
        Assert.Contains("Ahead 󰜘", lines[0]);
        Assert.Contains("Behind 󰜘", lines[0]);
        Assert.Contains("Branch Name ", lines[0]);
        Assert.Contains("Last commit ", lines[0]);

        Assert.True(lines[1].All(c => c == '|' || c == '-' || c == ' '));

        foreach (string line in lines.Skip(2))
        {
            var (ahead, behind) = GetAheadBehindFromString(line);

            Assert.True(ahead >= 0);
            Assert.True(behind >= 0);
        }
    }

    private (int ahead, int behind) GetAheadBehindFromString(string line)
    {
        int ahead = 0;
        int behind = 0;

        Match match = aheadBehindPattern().Match(line);

        if (!match.Success) throw new Exception("Failed to parse ahead/behind");
        
        ahead = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
        behind = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);

        return (ahead, behind);
    }

    [GeneratedRegex(@"\s*(\d+)\s*\|\s*(\d+)", RegexOptions.Compiled)]
    private static partial Regex aheadBehindPattern();
}