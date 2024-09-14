namespace CLI;

using Shared.TableData;
using Git.Options;
using CLI.Flags;
using Git.Base;

public class BranchTableAssembler
{
    private static readonly IGitRepository _gitBase = GitRepository.GetInstance();

    internal static List<GitBranch> AssembleBranchTable(IFlagCollection arguments)
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

    private static List<IOption> CreateOptions(IFlagCollection arguments)
    {
        List<IOption> options = [];

        if (arguments.Contains<AllFlag>())
        {
            IOption allOption = new BranchAllOptions(_gitBase);
            options.Add(allOption);

            return options;
        }

        if (arguments.Contains<RemoteFlag>())
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

    private static void AddContainsOptions(IFlagCollection arguments, CompositeOptionStrategy optionStrategies)
    {
        if (arguments.Contains<ContainsFlag>(out var containsFlag))
        {
            var value = containsFlag.Value;
            IOption containsOption = new ContainsOption(value);
            optionStrategies.AddStrategyOption(containsOption);

            return;
        }

        if (arguments.Contains<NoContainsFlag>(out var noContainsFlag))
        {
            var value = noContainsFlag.Value;
            IOption noContainsOption = new NoContainsOption(value);
            optionStrategies.AddStrategyOption(noContainsOption);
        }
    }

    private static void AddTrackOption(IFlagCollection arguments, CompositeOptionStrategy optionStrategies)
    {
        if (arguments.Contains<TrackFlag>(out var trackFlag))
        {
            var value = trackFlag.Value;
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

    private static void AddSortOption(IFlagCollection arguments, CompositeOptionStrategy optionStrategies)
    {
        IOption sortOption;

        if (arguments.Contains<SortFlag>(out var sortFlag))
        {
            var value = sortFlag.Value;

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

    private static void AddPrintTopOption(IFlagCollection arguments, CompositeOptionStrategy optionStrategies)
    {
        if (!arguments.Contains<PrintTopFlag>(out var printTopFlag)) return;

        var topValue = Convert.ToInt32(printTopFlag.Value);
        IOption printTopOption = new TopOption(topValue);
        optionStrategies.AddStrategyOption(printTopOption);
    }

}