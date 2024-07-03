namespace CLI;

using Shared.TableData;
using Git.Options;
using CLI.Flags;

public class BranchTableAssembler
{
    internal static List<GitBranch> AssembleBranchTable(Dictionary<FlagType, string> argumetns)
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