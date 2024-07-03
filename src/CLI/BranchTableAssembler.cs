namespace CLI;

using Shared.TableData;
using Git.Options;
using CLI.Flags;

public class BranchTableAssembler
{
    internal static List<GitBranch> AssembleBranchTable(Dictionary<FlagType, string> arguments)
    {
        List<IOption> options = [];
        CompositeOptionStrategy optionStrategies = new(options);

        if (arguments.ContainsKey(FlagType.All))
        {
            IOption allOption = new BranchAllOptions();
            optionStrategies.AddStrategyOption(allOption);
        }
        else if (arguments.ContainsKey(FlagType.Remote))
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

        if (arguments.ContainsKey(FlagType.Contains))
        {
            var value = arguments[FlagType.Contains];

            IOption containsOption = new ContainsOption(value);
            optionStrategies.AddStrategyOption(containsOption);
        }
        else if (arguments.ContainsKey(FlagType.NoContains))
        {
            var value = arguments[FlagType.NoContains];

            IOption noContainsOption = new NoContainsOption(value);
            optionStrategies.AddStrategyOption(noContainsOption);
        }

        if (arguments.ContainsKey(FlagType.Track))
        {
            var value = arguments[FlagType.Track];
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
        if (arguments.ContainsKey(FlagType.Sort))
        {
            var value = arguments[FlagType.Sort];

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

        if (arguments.ContainsKey(FlagType.PrintTop))
        {
            var topValue = Convert.ToInt32(arguments[FlagType.PrintTop]);
            IOption printTopOption = new TopOption(topValue);
            optionStrategies.AddStrategyOption(printTopOption);
        }

        List<GitBranch> gitBranches = [];
        return optionStrategies.Execute(gitBranches);
    }

}