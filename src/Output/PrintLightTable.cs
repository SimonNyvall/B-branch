using TableData;

namespace Bbranch.Output;

public class PrintLightTable
{
    public static void Print(List<GitBranch> branches)
    {
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