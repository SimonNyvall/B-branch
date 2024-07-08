namespace CLI;

using Shared.TableData;
using Git.Options;
using CLI.Flags;
using Git.Base;

public class BranchTableAssembler
{
    private static readonly IGitBase _gitBase = GitBase.GetInstance();

    internal static List<GitBranch> AssembleBranchTable(Dictionary<FlagType, string> arguments)
    {
        List<IOption> options = CreateOptions(arguments);
        CompositeOptionStrategy optionStrategies = new(options);

        AddLastCommitOption(optionStrategies);
        AddContainsOptions(arguments, optionStrategies);
        AddTrackOption(arguments, optionStrategies);
        AddWorkingBranchOption(optionStrategies);
        AddSortOption(arguments, optionStrategies);
        AddDescriptionOption(optionStrategies);
        AddPrintTopOption(arguments, optionStrategies);

        return optionStrategies.Execute([]);
    }

    private static List<IOption> CreateOptions(Dictionary<FlagType, string> arguments)
    {
        List<IOption> options = [];

        if (arguments.ContainsKey(FlagType.All))
        {
            IOption allOption = new BranchAllOptions(_gitBase);
            options.Add(allOption);

            return options;
        }

        if (arguments.ContainsKey(FlagType.Remote))
        {
            IOption remoteOption = new BranchRemoteOptions(_gitBase);
            options.Add(remoteOption);

            return options;
        }

        IOption localOption = new BranchLocalOptions(_gitBase);
        options.Add(localOption);

        return options;
    }

    private static void AddLastCommitOption(CompositeOptionStrategy optionStrategies)
    {
        IOption lastCommitOption = new SetLastCommitOptions(_gitBase);
        optionStrategies.AddStrategyOption(lastCommitOption);
    }

    private static void AddContainsOptions(Dictionary<FlagType, string> arguments, CompositeOptionStrategy optionStrategies)
    {
        if (arguments.ContainsKey(FlagType.Contains))
        {
            var value = arguments[FlagType.Contains];
            IOption containsOption = new ContainsOption(value);
            optionStrategies.AddStrategyOption(containsOption);

            return;
        }

        if (arguments.ContainsKey(FlagType.Nocontains))
        {
            var value = arguments[FlagType.Nocontains];
            IOption noContainsOption = new NoContainsOption(value);
            optionStrategies.AddStrategyOption(noContainsOption);
        }
    }

    private static void AddTrackOption(Dictionary<FlagType, string> arguments, CompositeOptionStrategy optionStrategies)
    {
        if (arguments.ContainsKey(FlagType.Track))
        {
            var value = arguments[FlagType.Track];
            IOption trackOption = new TrackAheadBehindOption(_gitBase, value);
            optionStrategies.AddStrategyOption(trackOption);

            return;
        }

        IOption aheadBehindOption = new DefaultAheadBehindOption(_gitBase);
        optionStrategies.AddStrategyOption(aheadBehindOption);
    }

    private static void AddWorkingBranchOption(CompositeOptionStrategy optionStrategies)
    {
        IOption workingBranchOption = new WorkingBranchOption(_gitBase);
        optionStrategies.AddStrategyOption(workingBranchOption);
    }

    private static void AddSortOption(Dictionary<FlagType, string> arguments, CompositeOptionStrategy optionStrategies)
    {
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

            return;
        }

        sortOption = new SortByLastCommitOptions();
        optionStrategies.AddStrategyOption(sortOption);
    }

    private static void AddDescriptionOption(CompositeOptionStrategy optionStrategies)
    {
        IOption descriptionOption = new DescriptionOption();
        optionStrategies.AddStrategyOption(descriptionOption);
    }

    private static void AddPrintTopOption(Dictionary<FlagType, string> arguments, CompositeOptionStrategy optionStrategies)
    {
        if (!arguments.ContainsKey(FlagType.Printtop)) return;

        var topValue = Convert.ToInt32(arguments[FlagType.Printtop]);
        IOption printTopOption = new TopOption(topValue);
        optionStrategies.AddStrategyOption(printTopOption);
    }

}