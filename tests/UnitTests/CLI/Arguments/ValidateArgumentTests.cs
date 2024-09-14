using CLI.Flags;
using CLI.ValidateArguments;

namespace Tests.Arguments;

public class ValidateArgumentTests
{
    [Fact]
    public void ValidateArguments_ShouldReturnSuccess_WithOnlyVersionArgument()
    {
        FlagCollection options = new();
        options.Add(IFlag<VersionFlag>.Create(null));

        Assert.True(Validate.ValidateOptions(options));
    }

    [Fact]
    public void ValidateArguments_ShouldRetrunError_WithVersionAndContainsArguments()
    {
        FlagCollection options = new();
        options.Add(IFlag<VersionFlag>.Create(null));
        options.Add(IFlag<ContainsFlag>.Create(null));

        Assert.False(Validate.ValidateOptions(options));
    }

    [Fact]
    public void ValidateArguments_ShouldReturnSuccess_WithContainsArgument()
    {
        FlagCollection options = new();
        options.Add(IFlag<ContainsFlag>.Create("main"));

        Assert.True(Validate.ValidateOptions(options));
    }

    [Fact]
    public void ValidateArguments_ShouldReturnError_WithContainsAndNoContainsArguments()
    {
        FlagCollection options = new();
        options.Add(IFlag<ContainsFlag>.Create(null));
        options.Add(IFlag<NoContainsFlag>.Create(null));

        Assert.False(Validate.ValidateOptions(options));
    }

    [Fact]
    public void ValidateArguments_ShouldReturnSuccess_WithAllArgument()
    {
        FlagCollection options = new();
        options.Add(IFlag<AllFlag>.Create(null));

        Assert.True(Validate.ValidateOptions(options));
    }

    [Fact]
    public void ValidateArguments_ShouldReturnSuccess_WithRemoteArgument()
    {
        FlagCollection options = new();
        options.Add(IFlag<RemoteFlag>.Create(null));

        Assert.True(Validate.ValidateOptions(options));
    }

    [Fact]
    public void ValidateArguments_ShouldReturnError_WithAllAndRemoteArguments()
    {
        FlagCollection options = new();
        options.Add(IFlag<AllFlag>.Create(null));
        options.Add(IFlag<RemoteFlag>.Create(null));

        Assert.False(Validate.ValidateOptions(options));
    }

    [Fact]
    public void ValidateArguments_ShoudlReturnSuccess_WithSortDateValueArgument()
    {
        FlagCollection options = new();
        options.Add(IFlag<SortFlag>.Create("date"));

        Assert.True(Validate.ValidateOptions(options));
    }

    [Fact]
    public void ValidateArguments_ShouldReturnSuccess_WithSortNameValueArgument()
    {
        FlagCollection options = new();
        options.Add(IFlag<SortFlag>.Create("name"));

        Assert.True(Validate.ValidateOptions(options));
    }

    [Fact]
    public void ValidateArguments_ShouldReturnSuccess_WithSortAheadValueArgument()
    {
        FlagCollection options = new();
        options.Add(IFlag<SortFlag>.Create("ahead"));

        Assert.True(Validate.ValidateOptions(options));
    }

    [Fact]
    public void ValidateArguments_ShouldReturnSuccess_WithSortBehindValueArgument()
    {
        FlagCollection options = new();
        options.Add(IFlag<SortFlag>.Create("behind"));

        Assert.True(Validate.ValidateOptions(options));
    }

    [Fact]
    public void ValidateArguments_ShouldReturnError_WithSortInvalidValueArgument()
    {
        FlagCollection options = new();
        options.Add(IFlag<SortFlag>.Create("invalid"));

        Assert.False(Validate.ValidateOptions(options));
    }

    [Fact]
    public void ValidateArguments_ShouldReturnSucccess_WithPrintTopValueArgument()
    {
        FlagCollection options = new();
        options.Add(IFlag<PrintTopFlag>.Create("5"));

        Assert.True(Validate.ValidateOptions(options));
    }

    [Fact]
    public void ValidateArguments_ShouldReturnError_WithPrintTopInvalidValueArgument()
    {
        FlagCollection options = new();
        options.Add(IFlag<PrintTopFlag>.Create("invalid"));

        Assert.False(Validate.ValidateOptions(options));
    }

    [Fact]
    public void ValidateArguments_ShouldReturnError_WithPrintTopNegativeValueArgument()
    {
        FlagCollection options = new();
        options.Add(IFlag<PrintTopFlag>.Create("-5"));

        Assert.False(Validate.ValidateOptions(options));
    }

    [Fact]
    public void ValidateArguments_ShouldReturnError_WithConstinsNullValueArgument()
    {
        FlagCollection options = new();
        options.Add(IFlag<ContainsFlag>.Create(null));

        Assert.False(Validate.ValidateOptions(options));
    }

    [Fact]
    public void ValidateArguments_ShouldReturnError_WithNoContainsNullValueArgument()
    {
        FlagCollection options = new();
        options.Add(IFlag<NoContainsFlag>.Create(null));

        Assert.False(Validate.ValidateOptions(options));
    }

    [Fact]
    public void ValidateArguments_ShouldReturnError_WithTrackNullValueArgument()
    {
        FlagCollection options = new();
        options.Add(IFlag<TrackFlag>.Create(null));

        Assert.False(Validate.ValidateOptions(options));
    }

    [Fact]
    public void ValidateArguments_ShouldReturnError_WithSortNullValueArgument()
    {
        FlagCollection options = new();
        options.Add(IFlag<SortFlag>.Create(null));

        Assert.False(Validate.ValidateOptions(options));
    }

    [Fact]
    public void ValidateArguments_ShouldReturnError_WithPrintTopNullValueArgument()
    {
        FlagCollection options = new();
        options.Add(IFlag<PrintTopFlag>.Create(null));

        Assert.False(Validate.ValidateOptions(options));
    }
}