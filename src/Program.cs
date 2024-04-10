using Bbranch.TablePrinter;
using Git.Base;
using Git.Options;
using TableData;

namespace Bbranch
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Dictionary<string, string> options = ParseArguments(args);

            ValidateOptions(options);

            HelpOptions.Handle(options);

            VersionOptions.Handle(options);

            List<BranchTableRow> branchTable = await AssembleBranchTable(options);

            Data.PrintBranchTable(branchTable, options);
        }

        private static async Task<List<BranchTableRow>> AssembleBranchTable(
            Dictionary<string, string> options
        )
        {
            GitBase gitBase = await GitBase.Initialize();

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

        private static Dictionary<string, string> ParseArguments(string[] args)
        {
            Dictionary<string, string> options = [];

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].StartsWith("--") || args[i].StartsWith('-'))
                {
                    string option = args[i].TrimStart('-');

                    if ((i + 1) < args.Length && !args[i + 1].StartsWith("--"))
                    {
                        options[option] = args[++i];
                        continue;
                    }

                    options[option] = "true";
                }
            }
            return options;
        }

        private static void ValidateOptions(Dictionary<string, string> options)
        {
            if (options.Count == 0)
            {
                return;
            }

            if (
                options.Keys.Any(opt =>
                    opt.Length == 1 && !HelpOptions.ValidArgs.Any(arg => arg.Item2 == opt)
                )
            )
            {
                Console.WriteLine("Invalid short option specified");
                Environment.Exit(1);
            }

            if (options.ContainsKey("version") && options.Any(opt => opt.Key != "version"))
            {
                Console.WriteLine("You cannot use --version with any other option");
                Environment.Exit(1);
            }

            if (options.ContainsKey("contains") && options.ContainsKey("no-contains"))
            {
                Console.WriteLine("You cannot use both --contains and --no-contains");
                Environment.Exit(1);
            }

            if (options.ContainsKey("all") && options.ContainsKey("remote"))
            {
                Console.WriteLine("You cannot use both --all and --remote");
                Environment.Exit(1);
            }
        }
    }
}
