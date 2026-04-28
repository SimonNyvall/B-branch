using Bbranch.CLI;
using Bbranch.CLI.Arguments.FlagSystem;
using Bbranch.CLI.Arguments.FlagSystem.Flags;
using Bbranch.GitService.Base;
using Bbranch.GitService.OptionStrategies.Common;
using Bbranch.GitService.OptionStrategies.Common.BranchStrategies;
using Bbranch.GitService.OptionStrategies.Common.ContainsStrategies;
using Bbranch.GitService.OptionStrategies.Common.SortStrategies;
using Bbranch.GitService.OptionStrategies.Shared.Setters;
using Bbranch.GitService.OptionStrategies.Shared.Strategies;
using FakeItEasy;

namespace Bbranch.Tests.CLI;

[Trait("Category", "Unit")]
public class BranchTableAssemblerTests
{
    private readonly IGitRepository _repositoryFake;

    public BranchTableAssemblerTests()
    {
        _repositoryFake = A.Fake<IGitRepository>();
    }

    [Fact]
    public void AssembleBranchTable_NoFlags_UsesLocalBranchOptions()
    {
        var arguments = new FlagCollection();

        var result = BranchTableAssembler.AssembleBranchTable(_repositoryFake, arguments);

        Assert.Equal(7, BranchTableAssembler._optionStrategies.Length);
        Assert.Collection(
            BranchTableAssembler._optionStrategies.Options,
            flag => Assert.IsType<BranchLocalOptions>(flag),
            flag => Assert.IsType<SetLastCommitOptions>(flag),
            flag => Assert.IsType<WorkingBranchOption>(flag),
            flag => Assert.IsType<DescriptionOption>(flag),
            flag => Assert.IsType<DefaultAheadBehindOption>(flag),
            flag => Assert.IsType<SortByLastCommitOptions>(flag),
            flag => Assert.IsType<SortByDetachedHeadOption>(flag)
        );
        Assert.NotNull(result);
    }

    [Fact]
    public void AssembleBranchTable_AllFlag_UsesAllBranchOptions()
    {
        var arguments = new FlagCollection { IFlag<AllFlag>.Create(null) };

        var result = BranchTableAssembler.AssembleBranchTable(_repositoryFake, arguments);

        Assert.Equal(7, BranchTableAssembler._optionStrategies.Length);
        Assert.Collection(
            BranchTableAssembler._optionStrategies.Options,
            flag => Assert.IsType<BranchAllOptions>(flag),
            flag => Assert.IsType<SetLastCommitOptions>(flag),
            flag => Assert.IsType<WorkingBranchOption>(flag),
            flag => Assert.IsType<DescriptionOption>(flag),
            flag => Assert.IsType<DefaultAheadBehindOption>(flag),
            flag => Assert.IsType<SortByLastCommitOptions>(flag),
            flag => Assert.IsType<SortByDetachedHeadOption>(flag)
        );
        Assert.NotNull(result);
    }

    [Fact]
    public void AssembleBranchTable_RemoteFlag_UsesRemoteBranchOptions()
    {
        var arguments = new FlagCollection { IFlag<RemoteFlag>.Create(null) };

        var result = BranchTableAssembler.AssembleBranchTable(_repositoryFake, arguments);

        Assert.Equal(7, BranchTableAssembler._optionStrategies.Length);
        Assert.Collection(
            BranchTableAssembler._optionStrategies.Options,
            flag => Assert.IsType<BranchRemoteOptions>(flag),
            flag => Assert.IsType<SetLastCommitOptions>(flag),
            flag => Assert.IsType<WorkingBranchOption>(flag),
            flag => Assert.IsType<DescriptionOption>(flag),
            flag => Assert.IsType<DefaultAheadBehindOption>(flag),
            flag => Assert.IsType<SortByLastCommitOptions>(flag),
            flag => Assert.IsType<SortByDetachedHeadOption>(flag)
        );
        Assert.NotNull(result);
    }

    [Fact]
    public void AssembleBranchTable_TrackFlag_AddsTrackAheadBehindOption()
    {
        var arguments = new FlagCollection { IFlag<TrackFlag>.Create(null) };

        var result = BranchTableAssembler.AssembleBranchTable(_repositoryFake, arguments);

        Assert.Equal(7, BranchTableAssembler._optionStrategies.Length);
        Assert.Collection(
            BranchTableAssembler._optionStrategies.Options,
            flag => Assert.IsType<BranchLocalOptions>(flag),
            flag => Assert.IsType<SetLastCommitOptions>(flag),
            flag => Assert.IsType<WorkingBranchOption>(flag),
            flag => Assert.IsType<DescriptionOption>(flag),
            flag => Assert.IsType<TrackAheadBehindOption>(flag),
            flag => Assert.IsType<SortByLastCommitOptions>(flag),
            flag => Assert.IsType<SortByDetachedHeadOption>(flag)
        );
        Assert.NotNull(result);
    }

    [Fact]
    public void AssembleBranchTable_ContainsFlag_AddsContainsOption()
    {
        var arguments = new FlagCollection { IFlag<ContainsFlag>.Create(null) };

        var result = BranchTableAssembler.AssembleBranchTable(_repositoryFake, arguments);

        Assert.Equal(8, BranchTableAssembler._optionStrategies.Length);
        Assert.Collection(
            BranchTableAssembler._optionStrategies.Options,
            flag => Assert.IsType<BranchLocalOptions>(flag),
            flag => Assert.IsType<SetLastCommitOptions>(flag),
            flag => Assert.IsType<WorkingBranchOption>(flag),
            flag => Assert.IsType<DescriptionOption>(flag),
            flag => Assert.IsType<DefaultAheadBehindOption>(flag),
            flag => Assert.IsType<SortByLastCommitOptions>(flag),
            flag => Assert.IsType<SortByDetachedHeadOption>(flag),
            flag => Assert.IsType<ContainsOption>(flag)
        );
        Assert.NotNull(result);
    }

    [Fact]
    public void AssembleBranchTable_NoContainsFlag_AddsNoContainsOption()
    {
        var arguments = new FlagCollection { IFlag<NoContainsFlag>.Create(null) };

        var result = BranchTableAssembler.AssembleBranchTable(_repositoryFake, arguments);

        Assert.Equal(8, BranchTableAssembler._optionStrategies.Length);
        Assert.Collection(
            BranchTableAssembler._optionStrategies.Options,
            flag => Assert.IsType<BranchLocalOptions>(flag),
            flag => Assert.IsType<SetLastCommitOptions>(flag),
            flag => Assert.IsType<WorkingBranchOption>(flag),
            flag => Assert.IsType<DescriptionOption>(flag),
            flag => Assert.IsType<DefaultAheadBehindOption>(flag),
            flag => Assert.IsType<SortByLastCommitOptions>(flag),
            flag => Assert.IsType<SortByDetachedHeadOption>(flag),
            flag => Assert.IsType<NoContainsOption>(flag)
        );
        Assert.NotNull(result);
    }

    [Fact]
    public void AssembleBranchTable_SortByName_AddsSortByNameOption()
    {
        var arguments = new FlagCollection { IFlag<SortFlag>.Create("name") };

        var result = BranchTableAssembler.AssembleBranchTable(_repositoryFake, arguments);

        Assert.Equal(7, BranchTableAssembler._optionStrategies.Length);
        Assert.Collection(
            BranchTableAssembler._optionStrategies.Options,
            flag => Assert.IsType<BranchLocalOptions>(flag),
            flag => Assert.IsType<SetLastCommitOptions>(flag),
            flag => Assert.IsType<WorkingBranchOption>(flag),
            flag => Assert.IsType<DescriptionOption>(flag),
            flag => Assert.IsType<DefaultAheadBehindOption>(flag),
            flag => Assert.IsType<SortByNameOptions>(flag),
            flag => Assert.IsType<SortByDetachedHeadOption>(flag)
        );
        Assert.NotNull(result);
    }

    [Fact]
    public void AssembleBranchTable_SortByAhead_AddsSortByAheadOption()
    {
        var arguments = new FlagCollection { IFlag<SortFlag>.Create("ahead") };

        var result = BranchTableAssembler.AssembleBranchTable(_repositoryFake, arguments);

        Assert.Equal(7, BranchTableAssembler._optionStrategies.Length);
        Assert.Collection(
            BranchTableAssembler._optionStrategies.Options,
            flag => Assert.IsType<BranchLocalOptions>(flag),
            flag => Assert.IsType<SetLastCommitOptions>(flag),
            flag => Assert.IsType<WorkingBranchOption>(flag),
            flag => Assert.IsType<DescriptionOption>(flag),
            flag => Assert.IsType<DefaultAheadBehindOption>(flag),
            flag => Assert.IsType<SortByAheadOptions>(flag),
            flag => Assert.IsType<SortByDetachedHeadOption>(flag)
        );
        Assert.NotNull(result);
    }

    [Fact]
    public void AssembleBranchTable_SortByBehind_AddsSortByBehindOption()
    {
        var arguments = new FlagCollection { IFlag<SortFlag>.Create("behind") };

        var result = BranchTableAssembler.AssembleBranchTable(_repositoryFake, arguments);

        Assert.Equal(7, BranchTableAssembler._optionStrategies.Length);
        Assert.Collection(
            BranchTableAssembler._optionStrategies.Options,
            flag => Assert.IsType<BranchLocalOptions>(flag),
            flag => Assert.IsType<SetLastCommitOptions>(flag),
            flag => Assert.IsType<WorkingBranchOption>(flag),
            flag => Assert.IsType<DescriptionOption>(flag),
            flag => Assert.IsType<DefaultAheadBehindOption>(flag),
            flag => Assert.IsType<SortByBehindOptions>(flag),
            flag => Assert.IsType<SortByDetachedHeadOption>(flag)
        );
        Assert.NotNull(result);
    }

    [Fact]
    public void AssembleBranchTable_SortByDate_AddsSortByLastCommitOption()
    {
        var arguments = new FlagCollection { IFlag<SortFlag>.Create("date") };

        var result = BranchTableAssembler.AssembleBranchTable(_repositoryFake, arguments);

        Assert.Equal(7, BranchTableAssembler._optionStrategies.Length);
        Assert.Collection(
            BranchTableAssembler._optionStrategies.Options,
            flag => Assert.IsType<BranchLocalOptions>(flag),
            flag => Assert.IsType<SetLastCommitOptions>(flag),
            flag => Assert.IsType<WorkingBranchOption>(flag),
            flag => Assert.IsType<DescriptionOption>(flag),
            flag => Assert.IsType<DefaultAheadBehindOption>(flag),
            flag => Assert.IsType<SortByLastCommitOptions>(flag),
            flag => Assert.IsType<SortByDetachedHeadOption>(flag)
        );
        Assert.NotNull(result);
    }

    [Fact]
    public void AssembleBranchTable_PrintTopFlag_AddsTopOption()
    {
        var arguments = new FlagCollection { IFlag<PrintTopFlag>.Create("5") };

        var result = BranchTableAssembler.AssembleBranchTable(_repositoryFake, arguments);

        Assert.Equal(8, BranchTableAssembler._optionStrategies.Length);
        Assert.Collection(
            BranchTableAssembler._optionStrategies.Options,
            flag => Assert.IsType<BranchLocalOptions>(flag),
            flag => Assert.IsType<SetLastCommitOptions>(flag),
            flag => Assert.IsType<WorkingBranchOption>(flag),
            flag => Assert.IsType<DescriptionOption>(flag),
            flag => Assert.IsType<DefaultAheadBehindOption>(flag),
            flag => Assert.IsType<SortByLastCommitOptions>(flag),
            flag => Assert.IsType<SortByDetachedHeadOption>(flag),
            flag => Assert.IsType<TopOption>(flag)
        );
        Assert.NotNull(result);
    }
}
