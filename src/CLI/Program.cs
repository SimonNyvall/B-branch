using Bbranch.CLI;
using Bbranch.CLI.Output;
using Bbranch.CLI.Options;
using Bbranch.CLI.Arguments;
using Bbranch.CLI.Arguments.FlagSystem.Flags;
using Bbranch.Shared.TableData;

if (!Parse.TryParseOptions(args, out var options))
{
    HelpOption.Execute();
}

if (!Validate.ValidateOptions(options))
{
    return;
}

if (options.Contains<HelpFlag>())
{
    HelpOption.Execute();
}

if (options.Contains<VersionFlag>())
{
    VersionOption.Execute();
}

HashSet<GitBranch> branchTable = BranchTableAssembler.AssembleBranchTable(options);

PageBehaviour shouldPage = PageBehaviour.Auto;

if (options.Contains<PagerFlag>())
{
    shouldPage = PageBehaviour.Paginate;
}
else if (options.Contains<NoPagerFlag>())
{
    shouldPage = PageBehaviour.None;
}

if (options.Contains<quietFlag>())
{
    PrintLightTable.Print(branchTable, shouldPage);

    return;
}

PrintFullTable.Print(branchTable, shouldPage);