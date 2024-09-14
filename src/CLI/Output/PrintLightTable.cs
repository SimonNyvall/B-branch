using Bbranch.Shared.TableData;

namespace Bbranch.CLI.Output;

public class PrintLightTable
{
    public static void Print(List<GitBranch> branches)
    {
        if (branches.Count == 0)
        {
            Console.WriteLine("No branches found");
            return;
        }

        foreach (var branch in branches)
        {
            if (branch.Branch.IsWorkingBranch)
            {
                PrintWorkingBranch(branch);
                continue;
            }

            Console.WriteLine($"  {branch.Branch.Name}");
        }

    }

    private static void PrintWorkingBranch(GitBranch branch)
    {
        Console.Write("* ");
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine(branch.Branch.Name);
        Console.ResetColor();
    }
}