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
    return;
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

if (options.Contains<QuiteFlag>())
{
    PrintLightTable.Print(branchTable);

    return;
}

PrintFullTable.Print(branchTable);

