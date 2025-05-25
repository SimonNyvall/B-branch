using Bbranch.CLI.Arguments;
using Bbranch.CLI.Arguments.FlagSystem;
using Bbranch.CLI.Arguments.FlagSystem.Flags;

namespace Bbranch.Tests.CLI.Arguments;

public class ValidateArgumentTests
{
    [Fact]
    public void Given_Validate_When_ValidateArgumentsRun_Then_Return_Success_WithOnlyVersionArgument()
    {
        FlagCollection options =
        [
            IFlag<VersionFlag>.Create(null)
        ];

        Assert.True(Validate.ValidateOptions(options));
    }

    [Fact]
    public void Given_Validate_When_ValidateArgumentsRun_Then_Return_Error_WithVersionAndContainsArguments()
    {
        FlagCollection options =
        [
            IFlag<VersionFlag>.Create(null),
            IFlag<ContainsFlag>.Create(null)
        ];

        Assert.False(Validate.ValidateOptions(options));
    }

    [Fact]
    public void Given_Validate_When_ValidateArgumentsRun_Then_Return_Success_WithContainsArgument()
    {
        FlagCollection options =
        [
            IFlag<ContainsFlag>.Create("main")
        ];

        Assert.True(Validate.ValidateOptions(options));
    }

    [Fact]
    public void Given_Validate_When_ValidateArgumentsRun_Then_Return_Error_WithContainsAndNoContainsArguments()
    {
        FlagCollection options =
        [
            IFlag<ContainsFlag>.Create(null),
            IFlag<NoContainsFlag>.Create(null)
        ];

        Assert.False(Validate.ValidateOptions(options));
    }

    [Fact]
    public void Given_Validate_When_ValidateArgumentsRun_Then_Return_Success_WithAllArgument()
    {
        FlagCollection options =
        [
            IFlag<AllFlag>.Create(null)
        ];

        Assert.True(Validate.ValidateOptions(options));
    }

    [Fact]
    public void Given_Validate_When_ValidateArgumentsRun_Then_Return_Success_WithRemoteArgument()
    {
        FlagCollection options =
        [
            IFlag<RemoteFlag>.Create(null)
        ];

        Assert.True(Validate.ValidateOptions(options));
    }

    [Fact]
    public void Given_Validate_When_ValidateArgumentsRun_Then_Return_Error_WithAllAndRemoteArguments()
    {
        FlagCollection options =
        [
            IFlag<AllFlag>.Create(null),
            IFlag<RemoteFlag>.Create(null)
        ];

        Assert.False(Validate.ValidateOptions(options));
    }

    [Fact]
    public void Given_Validate_When_ValidateArgumentsRun_Then_Return_Success_WithSortDateValueArgument()
    {
        FlagCollection options =
        [
            IFlag<SortFlag>.Create("date")
        ];

        Assert.True(Validate.ValidateOptions(options));
    }

    [Fact]
    public void Given_Validate_When_ValidateArgumentsRun_Then_Return_Success_WithSortNameValueArgument()
    {
        FlagCollection options =
        [
            IFlag<SortFlag>.Create("name")
        ];

        Assert.True(Validate.ValidateOptions(options));
    }

    [Fact]
    public void Given_Validate_When_ValidateArgumentsRun_Then_Return_Success_WithSortAheadValueArgument()
    {
        FlagCollection options =
        [
            IFlag<SortFlag>.Create("ahead")
        ];

        Assert.True(Validate.ValidateOptions(options));
    }

    [Fact]
    public void Given_Validate_When_ValidateArgumentsRun_Then_Return_Success_WithSortBehindValueArgument()
    {
        FlagCollection options =
        [
            IFlag<SortFlag>.Create("behind")
        ];

        Assert.True(Validate.ValidateOptions(options));
    }

    [Fact]
    public void Given_Validate_When_ValidateArgumentsRun_Then_Return_Error_WithSortInvalidValueArgument()
    {
        FlagCollection options =
        [
            IFlag<SortFlag>.Create("invalid")
        ];

        Assert.False(Validate.ValidateOptions(options));
    }

    [Fact]
    public void Given_Validate_When_ValidateArgumentsRun_Then_Return_Success_WithPrintTopValueArgument()
    {
        FlagCollection options =
        [
            IFlag<PrintTopFlag>.Create("5")
        ];

        Assert.True(Validate.ValidateOptions(options));
    }

    [Fact]
    public void Given_Validate_When_ValidateArgumentsRun_Then_Return_Error_WithPrintTopInvalidValueArgument()
    {
        FlagCollection options =
        [
            IFlag<PrintTopFlag>.Create("invalid")
        ];

        Assert.False(Validate.ValidateOptions(options));
    }

    [Fact]
    public void Given_Validate_When_ValidateArgumentsRun_Then_Return_Error_WithPrintTopNegativeValueArgument()
    {
        FlagCollection options =
        [
            IFlag<PrintTopFlag>.Create("-5")
        ];

        Assert.False(Validate.ValidateOptions(options));
    }

    [Fact]
    public void Given_Validate_When_ValidateArgumentsRun_Then_Return_Error_WithContainsNullValueArgument()
    {
        FlagCollection options =
        [
            IFlag<ContainsFlag>.Create(null)
        ];

        Assert.False(Validate.ValidateOptions(options));
    }

    [Fact]
    public void Given_Validate_When_ValidateArgumentsRun_Then_Return_Error_WithNoContainsNullValueArgument()
    {
        FlagCollection options =
        [
            IFlag<NoContainsFlag>.Create(null)
        ];

        Assert.False(Validate.ValidateOptions(options));
    }

    [Fact]
    public void Given_Validate_When_ValidateArgumentsRun_Then_Return_Error_WithTrackNullValueArgument()
    {
        FlagCollection options =
        [
            IFlag<TrackFlag>.Create(null)
        ];

        Assert.False(Validate.ValidateOptions(options));
    }

    [Fact]
    public void Given_Validate_When_ValidateArgumentsRun_Then_Return_Error_WithSortNullValueArgument()
    {
        FlagCollection options =
        [
            IFlag<SortFlag>.Create(null)
        ];

        Assert.False(Validate.ValidateOptions(options));
    }

    [Fact]
    public void Given_Validate_When_ValidateArgumentsRun_Then_Return_Error_WithPrintTopNullValueArgument()
    {
        FlagCollection options =
        [
            IFlag<PrintTopFlag>.Create(null)
        ];

        Assert.False(Validate.ValidateOptions(options));
    }

    [Fact]
    public void Given_Validate_When_ValidateArgumentsRun_Then_Return_Error_WithPagerAndNoPagerArguments()
    {
        FlagCollection options =
        [
            IFlag<PagerFlag>.Create(null),
            IFlag<NoPagerFlag>.Create(null)
        ];

        Assert.False(Validate.ValidateOptions(options));
    }

    [Fact]
    public void Given_Validate_When_ValidateArgumentsRun_Then_Return_Error_WithAllAndValueArguments()
    {
        FlagCollection options =
        [
            IFlag<AllFlag>.Create("value")
        ];

        Assert.False(Validate.ValidateOptions(options));
    }

    [Fact]
    public void Given_Validate_When_ValidateArgumentsRun_Then_Return_Error_WithHelpAndValueArguments()
    {
        FlagCollection options =
        [
            IFlag<HelpFlag>.Create("value")
        ];

        Assert.False(Validate.ValidateOptions(options));
    }

    [Fact]
    public void Given_Validate_When_ValidateArgumentsRun_Then_Return_Error_WithVersionAndValueArguments()
    {
        FlagCollection options =
        [
            IFlag<VersionFlag>.Create("value")
        ];

        Assert.False(Validate.ValidateOptions(options));
    }

    [Fact]
    public void Given_Validate_When_ValidateArgumentsRun_Then_Return_Error_WithRemoteAndValueArguments()
    {
        FlagCollection options =
        [
            IFlag<RemoteFlag>.Create("value")
        ];

        Assert.False(Validate.ValidateOptions(options));
    }

    [Fact]
    public void Given_Validate_When_ValidateArgumentsRun_Then_Return_Error_WithQuietAndValueArguments()
    {
        FlagCollection options =
        [
            IFlag<quietFlag>.Create("value")
        ];

        Assert.False(Validate.ValidateOptions(options));
    }

    [Fact]
    public void Given_Validate_When_ValidateArgumentsRun_Then_Return_Error_WithPagerAndValueArguments()
    {
        FlagCollection options =
        [
            IFlag<PagerFlag>.Create("value")
        ];

        Assert.False(Validate.ValidateOptions(options));
    }

    [Fact]
    public void Given_Validate_When_ValidateArgumentsRun_Then_Return_Error_WithNoPagerAndValueArguments()
    {
        FlagCollection options =
        [
            IFlag<NoPagerFlag>.Create("value")
        ];

        Assert.False(Validate.ValidateOptions(options));
    }
}