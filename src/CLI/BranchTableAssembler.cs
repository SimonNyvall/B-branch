using Bbranch.CLI.Arguments.FlagSystem;
using Bbranch.CLI.Arguments.FlagSystem.Flags;
using Bbranch.GitService.Base;
using Bbranch.GitService.OptionStrategies;
using Bbranch.GitService.OptionStrategies.Common;
using Bbranch.GitService.OptionStrategies.Common.BranchStrategies;
using Bbranch.GitService.OptionStrategies.Common.ContainsStrategies;
using Bbranch.GitService.OptionStrategies.Common.SortStrategies;
using Bbranch.GitService.OptionStrategies.Shared.Setters;
using Bbranch.GitService.OptionStrategies.Shared.Strategies;
using Bbranch.Shared.TableData;

namespace Bbranch.CLI;

public class BranchTableAssembler
{
    private static readonly IGitRepository _gitBase = GitRepository.GetInstance();

    internal static List<GitBranch> AssembleBranchTable(FlagCollection arguments)
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

    private static List<IOption> CreateOptions(FlagCollection arguments)
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

    private static void AddContainsOptions(FlagCollection arguments, CompositeOptionStrategy optionStrategies)
    {
        if (arguments.Contains<ContainsFlag>(out var containsFlag))
        {
            var value = containsFlag.Value!.ToString();

            IOption containsOption = new ContainsOption(value.ToString());
            optionStrategies.AddStrategyOption(containsOption);

            return;
        }

        if (arguments.Contains<NoContainsFlag>(out var noContainsFlag))
        {
            var value = noContainsFlag.Value;

            IOption noContainsOption = new NoContainsOption(value.ToString());
            optionStrategies.AddStrategyOption(noContainsOption);
        }
    }

    private static void AddTrackOption(FlagCollection arguments, CompositeOptionStrategy optionStrategies)
    {
        if (arguments.Contains<TrackFlag>(out var trackFlag))
        {
            var value = trackFlag.Value.ToString();

            IOption trackOption = new TrackAheadBehindOption(_gitBase, value.ToString());
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

    private static void AddSortOption(FlagCollection arguments, CompositeOptionStrategy optionStrategies)
    {
        IOption sortOption;

        if (arguments.Contains<SortFlag>(out var sortFlag))
        {
            var value = sortFlag.Value.ToString();

            if (value.ToString() == "name")
            {
                sortOption = new SortByNameOptions();
                optionStrategies.AddStrategyOption(sortOption);
            }
            else if (value.ToString() == "ahead")
            {
                sortOption = new SortByAheadOptions();
                optionStrategies.AddStrategyOption(sortOption);
            }
            else if (value.ToString() == "behind")
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

    private static void AddPrintTopOption(FlagCollection arguments, CompositeOptionStrategy optionStrategies)
    {
        if (!arguments.Contains<PrintTopFlag>(out var printTopFlag)) return;

        var topValue = Convert.ToInt32(printTopFlag.Value.ToString());
        IOption printTopOption = new TopOption(topValue);
        optionStrategies.AddStrategyOption(printTopOption);
    }

}