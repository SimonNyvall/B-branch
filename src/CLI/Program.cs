using Bbranch.Flags;
using Bbranch.ParseArguments;
using Bbranch.Output;
using Git.Options;
using CLI.Options;
using Shared.TableData;

namespace Bbranch;

public class Program
{
    public static void Main(string[] args)
    {
        Dictionary<FlagType, string> options = Parse.Arguments(args);

        if (options.ContainsKey(FlagType.Help))
        {
            HelpOptions.Execute();
        }

        if (options.ContainsKey(FlagType.Version))
        {
            VersionOptions.Execute();
        }

        List<GitBranch> branchTable = AssembleBranchTable(options);

        if (options.ContainsKey(FlagType.Quiet))
        {
            if (options.ContainsKey(FlagType.PrintTop))
            {
                PrintLightTable.Print(branchTable, int.Parse(options[FlagType.PrintTop]));
                return;
            }

            PrintLightTable.Print(branchTable, null);

            return;
        }

        if (options.ContainsKey(FlagType.PrintTop))
        {
            PrintFullTable.Print(branchTable, int.Parse(options[FlagType.PrintTop]));
            return;
        }

        PrintFullTable.Print(branchTable, null);
    }

    private static List<GitBranch> AssembleBranchTable(
        Dictionary<FlagType, string> argumetns
    )
    {
        List<IOption> options = [];
        CompositeOptionStrategy optionStrategies = new(options);

        if (argumetns.ContainsKey(FlagType.All))
        {
            IOption allOption = new BranchAllOptions();
            optionStrategies.AddStrategyOption(allOption);
        }
        else if (argumetns.ContainsKey(FlagType.Remote))
        {
            IOption remoteOption = new BranchRemoteOptions();
            optionStrategies.AddStrategyOption(remoteOption);
        }
        else 
        {
            IOption localOption = new BranchLocalOptions();
            optionStrategies.AddStrategyOption(localOption);
        }

        IOption lastCommitOption = new SetLastCommitOptions();
        optionStrategies.AddStrategyOption(lastCommitOption);

        if (argumetns.ContainsKey(FlagType.Contains))
        {
            var value = argumetns[FlagType.Contains];

            IOption containsOption = new ContainsOption(value);
            optionStrategies.AddStrategyOption(containsOption);
        }
        else if (argumetns.ContainsKey(FlagType.NoContains))
        {
            var value = argumetns[FlagType.NoContains];

            IOption noContainsOption = new NoContainsOption(value);
            optionStrategies.AddStrategyOption(noContainsOption);
        }

        // TODO maybe have the track option call the ahead behind option but have some validation check in this method
        // TODO add the working branch as a strategy, but make the others a shared strategy
        if (argumetns.ContainsKey(FlagType.Track))
        {
            var value = argumetns[FlagType.Track];
            IOption trackOption = new TrackAheadBehindOption(value);
            optionStrategies.AddStrategyOption(trackOption);
        }
        else 
        {
            IOption aheadBehindOption = new DefaultAheadBehindOption();
            optionStrategies.AddStrategyOption(aheadBehindOption);
        }

        IOption workingBranchOption = new WorkingBranchOption();
        optionStrategies.AddStrategyOption(workingBranchOption);

        IOption sortOption; 
        if (argumetns.ContainsKey(FlagType.Sort))
        {
            var value = argumetns[FlagType.Sort];

            if (value == "name")
            {
                sortOption = new SortByNameOptions();
                optionStrategies.AddStrategyOption(sortOption);
            }
            else if (value == "ahead")
            {
                sortOption = new SortByAheadOptions();
                optionStrategies.AddStrategyOption(sortOption);
            }
            else if (value == "behind")
            {
                sortOption = new SortByBehindOptions();
                optionStrategies.AddStrategyOption(sortOption);
            }
        }
        else 
        {
            sortOption = new SortByLastEditedOptions();
            optionStrategies.AddStrategyOption(sortOption);
        }
        

        IOption descriptionOption = new DescriptionOption();
        optionStrategies.AddStrategyOption(descriptionOption);

        List<GitBranch> gitBranches = [];
        return optionStrategies.Execute(gitBranches);
    }
}
