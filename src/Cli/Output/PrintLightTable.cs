using System.Text;
using Bbranch.Shared.TableData;

namespace Bbranch.CLI.Output;

public static class PrintLightTable
{
    public static void Print(HashSet<GitBranch> branches, string? lessCommandPath)
    {
        if (branches.Count == 0)
        {
            Console.WriteLine("No branches found");
            return;
        }

        var output = BuildOutput(branches);

        if (string.IsNullOrEmpty(lessCommandPath))
        {
            Console.Write(output);
            return;
        }

        Pager.StartLess(output, lessCommandPath);
    }

    private static string BuildOutput(HashSet<GitBranch> branches)
    {
        var stringBuilder = new StringBuilder();
        const string reset = "\x1b[0m";
        const string green = "\x1b[32m";

        foreach (var branch in branches)
        {
            if (branch.Branch.IsWorkingBranch)
            {
                stringBuilder.AppendLine($"* {green}{branch.Branch.Name}{reset}");
            }
            else
            {
                stringBuilder.AppendLine($"  {branch.Branch.Name}");
            }
        }

        return stringBuilder.ToString();
    }
}