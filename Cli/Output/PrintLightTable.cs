using System.Text;
using Bbranch.Cli.Assemblers;
using Bbranch.GitService.GitRepositories;
using Bbranch.GitService.OptionStrategies;
using Bbranch.Shared.TableData;

namespace Bbranch.CLI.Output;

public class PrintLightTable
{
    private readonly IPager _pager;

    public PrintLightTable(IPager pager)
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
        IAsyncEnumerable<List<GitBranch>> batch
    )
    {
        var stringBuilder = new StringBuilder();

        await foreach (var branches in batch)
        {
            foreach (var branch in branches)
            {
                if (branch.Branch.IsWorkingBranch)
                {
                    stringBuilder.Append(
                        $"* {Constants.green}{branch.Branch.Name}{Constants.reset}"
                    );
                }
                else if (branch.IsRemote)
                {
                    stringBuilder.Append($"  {Constants.red}{branch.Branch.Name}{Constants.reset}");
                }
                else if (branch.IsCheckoutWorktree)
                {
                    stringBuilder.Append(
                        $"+ {Constants.blue}{branch.Branch.Name}{Constants.reset}"
                    );
                }
                else
                {
                    stringBuilder.Append($"  {branch.Branch.Name}");
                }

                stringBuilder.Append('\n');
            }

            yield return stringBuilder.ToString();
        }
    }

    private static int GetWindowHeight() => Console.WindowHeight;
}
