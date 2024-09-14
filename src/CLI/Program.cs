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
        IFlagCollection options = new FlagCollection();

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
            HelpOptions.Execute();
        }

        if (options.Contains<VersionFlag>())
        {
            VersionOptions.Execute();
        }

        List<GitBranch> branchTable = BranchTableAssembler.AssembleBranchTable(options);

        if (options.Contains<QuiteFlag>())
        {
            PrintLightTable.Print(branchTable);

            return;
        }

        PrintFullTable.Print(branchTable);
    }

}
