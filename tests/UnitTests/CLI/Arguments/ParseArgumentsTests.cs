namespace Tests.Arguments;

using CLI.Flags;
using CLI.ParseArguments;

public class ParseArgumentsTests
{
    [Fact]
    public void ParseArguments_ShouldReturnVersionFlag_WithOnlyVersionArgument()
    {
        string[] args = ["--version"];

        IFlagCollection options = new FlagCollection();

        bool isSuccessful = Parse.TryParseOptions(args, out options);

        Assert.True(options.Contains<VersionFlag>());
        Assert.True(isSuccessful);
    }

    [Fact]
    public void ParseArguments_ShouldReturnContainsFlag_WithContainsArgument()
    {
        string[] args = ["--contains", "test"];

        IFlagCollection options = new FlagCollection();

        bool isSuccessful = Parse.TryParseOptions(args, out options);

        Assert.True(options.Contains<ContainsFlag>());
        Assert.True(isSuccessful);
    }

    [Fact]
    public void ParseArguments_ShouldReturnNoContainsFlag_WithNoContainsArgument()
    {
        string[] args = ["--no-contains", "test"];

        IFlagCollection options = new FlagCollection();

        bool isSuccessful = Parse.TryParseOptions(args, out options);

        Assert.True(options.Contains<NoContainsFlag>());
        Assert.True(isSuccessful);
    }

    [Fact]
    public void ParseArguments_ShouldReturnError_WithInvalidArgument()
    {
        string[] args = ["--invalid"];

        IFlagCollection options = new FlagCollection();

        bool isSuccessful = Parse.TryParseOptions(args, out options);

        Assert.False(isSuccessful);
    }

    [Fact]
    public void ParseArguments_ShouldReturnError_WithDuplicateArgument()
    {
        string[] args = ["--contains", "test", "--contains", "test"];

        IFlagCollection options = new FlagCollection();

        bool isSuccessful = Parse.TryParseOptions(args, out options);

        Assert.False(isSuccessful);
    }

    [Fact]
    public void ParseArguments_ShouldReturnError_WithInvalidArgumentAfterFlag()
    {
        string[] args = ["--contains", "--invalid"];

        IFlagCollection options = new FlagCollection();

        bool isSuccessful = Parse.TryParseOptions(args, out options);

        Assert.False(isSuccessful);
    }

    [Fact]
    public void ParseArguments_ShouldReturnError_WithInvalidArgumentAfterFlagShort()
    {
        string[] args = ["-c", "--invalid"];

        IFlagCollection options = new FlagCollection();

        bool isSuccessful = Parse.TryParseOptions(args, out options);

        Assert.False(isSuccessful);
    }

    [Fact]
    public void ParseArguments_ShouldReturnError_WithInvalidArgumentAfterFlagShortAndValue()
    {
        string[] args = ["-c", "test", "--invalid"];

        IFlagCollection options = new FlagCollection();

        bool isSuccessful = Parse.TryParseOptions(args, out options);

        Assert.False(isSuccessful);
    }

    [Fact]
    public void ParseArguments_ShouldReturnError_WithInvalidArgumentAfterFlagShortAndValueShort()
    {
        string[] args = ["-c", "test", "-i"];

        IFlagCollection options = new FlagCollection();

        bool isSuccessful = Parse.TryParseOptions(args, out options);

        Assert.False(isSuccessful);
    }

    [Fact]
    public void ParseArguments_ShouldReturnError_WithInvalidArgumentAfterFlagShortAndValueShortAndValue()
    {
        string[] args = ["-c", "test", "-i", "test"];

        IFlagCollection options = new FlagCollection();

        bool isSuccessful = Parse.TryParseOptions(args, out options);

        Assert.False(isSuccessful);
    }

    [Fact]
    public void ParseArguments_ShouldReturnError_WithInvalidArgumentAfterFlagShortAndValueShortAndValueShort()
    {
        string[] args = ["-c", "test", "-i", "test", "-t"];

        IFlagCollection options = new FlagCollection();

        bool isSuccessful = Parse.TryParseOptions(args, out options);

        Assert.False(isSuccessful);
    }

    [Fact]
    public void ParseArguments_ShouldReturnError_WithInvalidArgumentAfterFlagShortAndValueShortAndValueShortAndValue()
    {
        string[] args = ["-c", "test", "-i", "test", "-t", "test"];

        IFlagCollection options = new FlagCollection();

        bool isSuccessful = Parse.TryParseOptions(args, out options);

        Assert.False(isSuccessful);
    }

    [Fact]
    public void ParseArgument_ShouldReturnHelpFlag_WithOnlyHelpArgument()
    {
        string[] args = ["--help"];

        IFlagCollection options = new FlagCollection();

        bool isSuccessful = Parse.TryParseOptions(args, out options);

        Assert.True(options.Contains<HelpFlag>());
        Assert.True(isSuccessful);
    }
}