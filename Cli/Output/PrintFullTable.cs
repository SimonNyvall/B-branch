using System.Text;
using Bbranch.Cli.Assemblers;
using Bbranch.GitService.GitRepositories;
using Bbranch.GitService.OptionStrategies;
using Bbranch.Shared.TableData;

namespace Bbranch.CLI.Output;

public class PrintFullTable
{
    private readonly IPager _pager;

    public PrintFullTable(IPager pager)
    {
        _pager = pager;
    }

    public async Task Print(
        CompositeOptionStrategy compositeOptionStrategy,
        string? lessCommandPath,
        GitRepository gitRepository,
        BranchScope scope
    )
    {
        Console.OutputEncoding = Encoding.UTF8;

        var batchSize = GetWindowHeight();
        var batch = BranchAssembler.AssembleBranches(
            compositeOptionStrategy,
            gitRepository,
            scope,
            batchSize
        );

        var output = BuildOutput(batch);

        if (string.IsNullOrEmpty(lessCommandPath))
        {
            await foreach (var line in output)
            {
                Console.Write(line);
            }

            return;
        }

        await _pager.StartLess(output, lessCommandPath);
    }

    private static async IAsyncEnumerable<string> BuildOutput(
        IAsyncEnumerable<List<GitBranch>> branchBatches
    )
    {
        int minWidth = 15;
        int longest = minWidth;

        var line =
            $"\n--------- | ---------- | ----------------- | ----------- | ----------------\n";

        yield return BuildHeaders(longest);
        yield return line;

        await foreach (var branches in branchBatches)
        {
            foreach (var branch in branches)
            {
                var stringBuilder = new StringBuilder();

                var ahead = branch.AheadBehind.Ahead.ToString().PadRight(8);
                var behind = branch.AheadBehind.Behind.ToString().PadRight(9);
                var name = branch.Branch.Name.PadRight(longest);
                var lastCommit = PrintFormatter.GetTimePrefix(branch.LastCommit, DateTime.Now);
                var status = branch.Status.ToString();
                var desc = branch.Description ?? "";
                var mergeRequestUrl = branch.MergeRequestUrl;

                stringBuilder.Append(" ");
                stringBuilder.Append(ahead);
                stringBuilder.Append(" |  ");
                stringBuilder.Append(behind);
                stringBuilder.Append(" | ");

                stringBuilder.Append(lastCommit.PadRight(18));
                stringBuilder.Append("|  ");

                stringBuilder.Append(
                    string.IsNullOrEmpty(mergeRequestUrl)
                        ? status.PadRight(11)
                        : HyperLink(status, mergeRequestUrl).PadRight(11)
                );

                stringBuilder.Append("| ");

                if (branch.Branch.IsWorkingBranch)
                {
                    stringBuilder.Append($"{Constants.green} {name}{Constants.reset}");
                }
                else if (branch.IsRemote)
                {
                    stringBuilder.Append($"{Constants.red} {name}{Constants.reset}");
                }
                else if (branch.IsCheckoutWorktree)
                {
                    stringBuilder.Append($"{Constants.blue} {name}{Constants.reset}");
                }
                else
                {
                    stringBuilder.Append($" {name}");
                }

                stringBuilder.Append($"{Constants.gray}{desc}{Constants.reset}");

                stringBuilder.AppendLine();

                yield return stringBuilder.ToString();
            }
        }
    }

    private static string HyperLink(string text, string url)
    {
        return $"\x1b]8;;{url}\x1b\\{text}\x1b]8;;\x1b\\";
    }

    private static string BuildHeaders(int longest)
    {
        const string yellow = "\x1b[33m";
        const string reset = "\x1b[0m";

        const string aheadIcon = "\ueafc";
        const string behindIcon = "\ueafc";
        const string branchIcon = "\ue725";
        const string commitIcon = "\ue729";

        var isUsingNerdFonts = IsUserUsingNerdFonts();

        string branchHeader = isUsingNerdFonts
            ? $"Branch name {branchIcon}".PadRight(longest)
            : "Branch name".PadRight(longest);

        // TODO: fix the nerd font header for status
        string line = isUsingNerdFonts
            ? $"{yellow} Ahead {aheadIcon}  {reset}|{yellow}  Behind {behindIcon}  |{yellow}  Last commit {commitIcon} {reset}"
            : $"{yellow} Ahead    {reset}|{yellow}  Behind    {reset}|{yellow}  Last commit   {reset}   |  {yellow}Status{reset}     | {yellow} {branchHeader}{reset}";

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

        return gitConfigLines.Any(line => line.Contains("\tuseNerdFonts = true"));
    }

    private static int GetWindowHeight()
    {
        const int headerHeight = 2;
        return Console.WindowHeight - headerHeight;
    }
}
