using Bbranch.ParseArguments;
using Bbranch.TablePrinter;
using Bbranch.ValidateArguments;
using Git.Base;
using Git.Options;
using TableData;

namespace Bbranch;

public class Program
{
    public static async Task Main(string[] args)
    {
        Dictionary<string, string> options = Parse.Arguments(args);

        if (Validate.Arguments(options) == Result.Error)
        {
            HelpOptions.Handle(options);
            Environment.Exit(1);
        }

        HelpOptions.Handle(options);

        VersionOptions.Handle(options);

        List<BranchTableRow> branchTable = await AssembleBranchTable(options);

        Data.PrintBranchTable(branchTable, options);
    }

    private static async Task<List<BranchTableRow>> AssembleBranchTable(
        Dictionary<string, string> options
    )
    {
        await GitBase.Initialize();

        List<GitBranch> branches = [];

        BranchOptions.GetBranches(options, ref branches);

        ContainsOptions.GetBranches(options, ref branches);

        string workingBranch = await GitBase.TryGetWorkingBranch();

        List<BranchTableRow> branchTable = await TrackOptions.GetBranches(
            options,
            branches,
            workingBranch
        );

        SortOptions.GetBranches(branchTable, options, ref branchTable);

        return branchTable;
    }
}
