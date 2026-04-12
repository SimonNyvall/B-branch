using Bbranch.CLI;
using Bbranch.CLI.Output;
using Bbranch.CLI.Options;
using Bbranch.CLI.Arguments;
using Bbranch.CLI.Arguments.FlagSystem.Flags;
using Bbranch.Shared.TableData;

var arguments = new List<string>(args);
bool foundLessCommand = true;

if (arguments.Count < 1 || !File.Exists(arguments[0]))
{
    PrintWarning("Less command does not exist");

    arguments.Remove("--pager");
    arguments.Add("--no-pager");

    foundLessCommand = false;
}

if (!Parse.TryParseOptions(arguments.ToArray(), out var options))
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

var lessCommandPath = foundLessCommand
    ? arguments[0]
    : null;

if (options.Contains<quietFlag>())
{
    PrintLightTable.Print(branchTable, lessCommandPath);
    return;
}

PrintFullTable.Print(branchTable, lessCommandPath);

void PrintWarning(string message)
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine($"{message}\n");
    Console.ResetColor();
}