using Bbranch.CLI;
using Bbranch.CLI.Arguments;
using Bbranch.CLI.Arguments.FlagSystem.Flags;
using Bbranch.CLI.Options;
using Bbranch.CLI.Output;
using Bbranch.GitService.Base;
using Bbranch.Shared.TableData;

var arguments = new List<string>(args);
bool foundLessCommand = true;

if (arguments.Count < 1 || !File.Exists(arguments[0]))
{
    PrintWarning("Less command does not exist");
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

var gitRepository = GitRepository.GetInstance();

HashSet<GitBranch> branchTable = BranchTableAssembler.AssembleBranchTable(gitRepository, options);

var lessCommandPath = foundLessCommand ? arguments[0] : null;

var pager = new Pager();

if (options.Contains<quietFlag>())
{
    var quietOutput = new PrintLightTable(pager);
    quietOutput.Print(branchTable, lessCommandPath);
    return;
}

var fullOutput = new PrintFullTable(pager);
fullOutput.Print(branchTable, lessCommandPath);

static void PrintWarning(string message)
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine($"{message}\n");
    Console.ResetColor();
}
