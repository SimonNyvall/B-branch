using CLI.Flags;
using CLI.ParseArguments;
using CLI.Output;
using CLI.Options;
using CLI.ValidateArguments;
using Shared.TableData;

namespace CLI;

public class Program
{
    public static void Main(string[] args)
    {
        Dictionary<FlagType, string> options = [];

        if (!Parse.TryParseOptions(args, out options))
        {
            return;
        }

        if (!Validate.ValidateOptions(options))
        {
            return;
        }

        if (options.ContainsKey(FlagType.Help))
        {
            HelpOptions.Execute();
        }

        if (options.ContainsKey(FlagType.Version))
        {
            VersionOptions.Execute();
        }

        List<GitBranch> branchTable = BranchTableAssembler.AssembleBranchTable(options);

        if (options.ContainsKey(FlagType.Quiet))
        {
            PrintLightTable.Print(branchTable);

            return;
        }

        PrintFullTable.Print(branchTable);
    }

}
