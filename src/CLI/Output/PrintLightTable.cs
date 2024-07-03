using Shared.TableData;

namespace Bbranch.Output;

public class PrintLightTable
{
    public static void Print(List<GitBranch> branches, int? top)
    {
        if (branches.Count == 0)
        {
            Console.WriteLine("No branches found");
            return;
        }

        int count = 0;

        foreach (var branch in branches)
        {
            if (top.HasValue && count == top)
            {
                break;
            }

            count++;

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