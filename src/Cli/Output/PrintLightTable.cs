using System.Text;
using Bbranch.Shared.TableData;

namespace Bbranch.CLI.Output;

public class PrintLightTable
{
    private readonly IPager _pager;

    public PrintLightTable(IPager pager)
    {
        _pager = pager;
    }

    public void Print(HashSet<GitBranch> branches, string? lessCommandPath)
    {
        if (branches.Count == 0)
        {
            return;
        }

        var output = BuildOutput(branches);

        if (string.IsNullOrEmpty(lessCommandPath))
        {
            Console.Write(output);
            return;
        }

        _pager.StartLess(output, lessCommandPath);
    }

    private static string BuildOutput(HashSet<GitBranch> branches)
    {
        var stringBuilder = new StringBuilder();
        const string reset = "\x1b[0m";
        const string green = "\x1b[32m";
        const string red = "\x1b[31m";
        const string blue = "\x1b[36m";

        var longestBranchName = branches.Max(x => x.Branch.Name.Length);

        foreach (var branch in branches)
        {
            if (branch.Branch.IsWorkingBranch)
            {
                stringBuilder.AppendLine($"* {green}{branch.Branch.Name}{reset}");
            }
            else if (branch.IsRemote)
            {
                stringBuilder.AppendLine($"  {red}{branch.Branch.Name}{reset}");
            }
            else if (branch.IsCheckoutWorktree)
            {
                stringBuilder.AppendLine($"+ {blue}{branch.Branch.Name}{reset}");
            }
            else
            {
                stringBuilder.AppendLine($"  {branch.Branch.Name}");
            }
        }

        return stringBuilder.ToString();
    }
}
