using Bbranch.CLI.Arguments;
using Bbranch.CLI.Arguments.FlagSystem;
using Bbranch.CLI.Arguments.FlagSystem.Flags;
using Bbranch.Cli.Assemblers;
using Bbranch.CLI.Options;
using Bbranch.CLI.Output;
using Bbranch.GitService.GitRepositories;

var arguments = new List<string>(args);
bool foundLessCommand = true;

if (arguments.Count < 1 || !File.Exists(arguments[0]))
{
    PrintWarning("The less command was not found");
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

var gitRepository = await GitRepository.GetInstance();

var strategies = OptionAssembler.AssembleOptions(gitRepository, options);

var lessCommandPath = foundLessCommand ? arguments[0] : null;

var pager = new Pager();

if (options.Contains<quietFlag>())
{
    var quietOutput = new PrintLightTable(pager);
    await quietOutput.Print(strategies, lessCommandPath, gitRepository, GetBranchScope(options));
    return;
}

var fullOutput = new PrintFullTable(pager);
await fullOutput.Print(strategies, lessCommandPath, gitRepository, GetBranchScope(options));

static void PrintWarning(string message)
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine($"{message}\n");
    Console.ResetColor();
}

static BranchScope GetBranchScope(FlagCollection flags)
{
    if (flags.Contains<AllFlag>())
    {
        return BranchScope.Both;
    }

    if (flags.Contains<RemoteFlag>())
    {
        return BranchScope.Remote;
    }

    return BranchScope.Local;
}
