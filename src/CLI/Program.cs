using Bbranch.CLI;
using Bbranch.CLI.Output;
using Bbranch.CLI.Options;
using Bbranch.CLI.Arguments;
using Bbranch.CLI.Arguments.FlagSystem;
using Bbranch.CLI.Arguments.FlagSystem.Flags;
using Bbranch.Shared.TableData;

FlagCollection options = [];

if (!Parse.TryParseOptions(args, out options))
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

List<GitBranch> branchTable = BranchTableAssembler.AssembleBranchTable(options);

PageBehaviour shouldPage;

if (options.Contains<PagerFlag>())
{
    shouldPage = PageBehaviour.Paginate;
}
else if (options.Contains<NoPagerFlag>())
{
    shouldPage = PageBehaviour.None;
}
else
{
    shouldPage = PageBehaviour.Auto;
}

if (options.Contains<quietFlag>())
{
    PrintLightTable.Print(branchTable);

    return;
}

PrintFullTable.Print(branchTable, shouldPage);

