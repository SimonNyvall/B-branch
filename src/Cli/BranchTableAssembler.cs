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

public static class BranchTableAssembler
{
    private static IGitRepository _gitRepository = null!;
    internal static CompositeOptionStrategy _optionStrategies = null!;

    internal static HashSet<GitBranch> AssembleBranchTable(
        IGitRepository gitRepository,
        FlagCollection arguments
    )
    {
        _gitRepository = gitRepository;

        List<IOption> options = CreateOptions(arguments);
        _optionStrategies = new(options);

        AddLastCommitOption();
        AddWorkingBranchOption();
        AddDescriptionOption();
        AddTrackOption(arguments);
        AddSortOption(arguments);
        AddContainsOptions(arguments);
        AddPrintTopOption(arguments);

        return _optionStrategies.Execute([]);
    }

    private static List<IOption> CreateOptions(FlagCollection arguments)
    {
        List<IOption> options = [];

        if (arguments.Contains<AllFlag>())
        {
            IOption allOption = new BranchAllOptions(_gitRepository);
            options.Add(allOption);

            return options;
        }

        if (arguments.Contains<RemoteFlag>())
        {
            IOption remoteOption = new BranchRemoteOptions(_gitRepository);
            options.Add(remoteOption);

            return options;
        }

        IOption localOption = new BranchLocalOptions(_gitRepository);
        options.Add(localOption);

        return options;
    }

    private static void AddLastCommitOption()
    {
        IOption lastCommitOption = new SetLastCommitOptions(_gitRepository);
        _optionStrategies.AddStrategyOption(lastCommitOption);
    }

    private static void AddContainsOptions(FlagCollection arguments)
    {
        if (arguments.Contains<ContainsFlag>(out var containsFlag))
        {
            var value = containsFlag.Value!.ToString();

            IOption containsOption = new ContainsOption(value);
            _optionStrategies.AddStrategyOption(containsOption);

            return;
        }

        if (arguments.Contains<NoContainsFlag>(out var noContainsFlag))
        {
            var value = noContainsFlag.Value;

            IOption noContainsOption = new NoContainsOption(value.ToString());
            _optionStrategies.AddStrategyOption(noContainsOption);
        }
    }

    private static void AddTrackOption(FlagCollection arguments)
    {
        if (arguments.Contains<TrackFlag>(out var trackFlag))
        {
            var value = trackFlag.Value.ToString();

            IOption trackOption = new TrackAheadBehindOption(_gitRepository, value);
            _optionStrategies.AddStrategyOption(trackOption);

            return;
        }

        IOption aheadBehindOption = new DefaultAheadBehindOption(_gitRepository);
        _optionStrategies.AddStrategyOption(aheadBehindOption);
    }

    private static void AddWorkingBranchOption()
    {
        IOption workingBranchOption = new WorkingBranchOption(_gitRepository);
        _optionStrategies.AddStrategyOption(workingBranchOption);
    }

    private static void AddSortOption(FlagCollection arguments)
    {
        IOption sortOption;

        if (arguments.Contains<SortFlag>(out var sortFlag))
        {
            var value = sortFlag.Value.ToString();

            if (value == "name")
            {
                sortOption = new SortByNameOptions();
                _optionStrategies.AddStrategyOption(sortOption);
            }
            else if (value == "ahead")
            {
                sortOption = new SortByAheadOptions();
                _optionStrategies.AddStrategyOption(sortOption);
            }
            else if (value == "behind")
            {
                sortOption = new SortByBehindOptions();
                _optionStrategies.AddStrategyOption(sortOption);
            }
            else if (value == "date")
            {
                sortOption = new SortByLastCommitOptions();
                _optionStrategies.AddStrategyOption(sortOption);
            }
        }
        else
        {
            sortOption = new SortByLastCommitOptions();
            _optionStrategies.AddStrategyOption(sortOption);
        }

        var detachedHeadSortOption = new SortDetachedHeadOption();
        _optionStrategies.AddStrategyOption(detachedHeadSortOption);
    }

    private static void AddDescriptionOption()
    {
        IOption descriptionOption = new DescriptionOption();
        _optionStrategies.AddStrategyOption(descriptionOption);
    }

    private static void AddPrintTopOption(FlagCollection arguments)
    {
        if (!arguments.Contains<PrintTopFlag>(out var printTopFlag))
            return;

        var topValue = Convert.ToInt32(printTopFlag.Value.ToString());
        IOption printTopOption = new TopOption(topValue);
        _optionStrategies.AddStrategyOption(printTopOption);
    }
}
