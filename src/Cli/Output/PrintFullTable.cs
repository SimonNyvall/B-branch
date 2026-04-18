using System.Text;
using Bbranch.Shared.TableData;

namespace Bbranch.CLI.Output;

public static class PrintFullTable
{
    public static void Print(HashSet<GitBranch> branches, string? lessCommandPath)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

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
        const string gray = "\x1b[90m";

        int minWidth = 14;
        int longest = Math.Max(minWidth, branches.Max(x => x.Branch.Name.Length + 2));

        //stringBuilder.AppendLine(BuildHeaders(longest));

        //stringBuilder.AppendLine(
        //    $"--------- | ---------- | {new string('-', longest)} | ----------------"
        //);

        Console.WriteLine(BuildHeaders(longest));
        Console.WriteLine(
            $"--------- | ---------- | {new string('-', longest)} | ----------------"
        );

        foreach (var branch in branches)
        {
            var ahead = branch.AheadBehind.Ahead.ToString().PadRight(8);
            var behind = branch.AheadBehind.Behind.ToString().PadRight(9);
            var name = branch.Branch.Name.PadRight(longest);
            var lastCommit = PrintFormater.GetTimePrefix(branch.LastCommit, DateTime.Now);
            var desc = branch.Description ?? "";

            stringBuilder.Append(" ");
            stringBuilder.Append(ahead);
            stringBuilder.Append(" |  ");
            stringBuilder.Append(behind);
            stringBuilder.Append(" | ");

            if (branch.Branch.IsWorkingBranch)
            {
                stringBuilder.Append($"{green} {name}{reset}");
            }
            else
            {
                stringBuilder.Append($" {name}");
            }

            stringBuilder.Append("|  ");
            stringBuilder.Append(lastCommit);
            stringBuilder.Append(" ");
            stringBuilder.Append($"    {gray}{desc}{reset}");

            stringBuilder.AppendLine();
        }

        return stringBuilder.ToString();
    }

    private static string BuildHeaders(int longest)
    {
        const string yellow = "\x1b[33m";
        const string reset = "\x1b[0m";

        const string aheadIcon = "\ueafc";
        const string behindIcon = "\ueafc";
        const string branchIcon = "⎇";
        const string commitIcon = "●";

        string branchHeader = "Branch name".PadRight(longest);

        string line = IsUserUsingNerdFonts()
            ? $"{yellow} Ahead {aheadIcon}  {reset}|{yellow} Behind {behindIcon}  {reset}|{yellow} {branchHeader}{branchIcon}{reset}|{yellow} Last commit {commitIcon} {reset}"
            : $"{yellow} Ahead    {reset}|{yellow}  Behind    {reset}|{yellow}  {branchHeader}{reset}|{yellow}  Last commit   {reset}";

        return line;
    }

    private static bool IsUserUsingNerdFonts()
    {
        string gitConfigPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".gitconfig"
        );

        if (!File.Exists(gitConfigPath))
            return false;

        string[] gitConfigLines = File.ReadAllLines(gitConfigPath);

        Environment.SetEnvironmentVariable("LESSCHARSET", "utf-8");
        //Environment.SetEnvironmentVariable("LESSUTFCHARDEF", "8bcccbcc18b95.b:");

        return gitConfigLines.Any(line => line.Contains("\tuseNerdFonts = true"));
    }

    private 
}
