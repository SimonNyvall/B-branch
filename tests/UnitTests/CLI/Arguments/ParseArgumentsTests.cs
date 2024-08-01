namespace Tests.Arguments;

using CLI.Flags;
using CLI.ParseArguments;

public class ParseArgumentsTests
{
    [Fact]
    public void ParseArguments_ShouldReturnVersionFlag_WithOnlyVersionArgument()
    {
        string[] args = ["--version"];

        Dictionary<FlagType, string> options = [];

        bool isSuccessful = Parse.TryParseOptions(args, out options);

        Assert.Equal(FlagType.Version, options.Keys.First());
        Assert.True(isSuccessful);
    }

    [Fact]
    public void ParseArguments_ShouldReturnContainsFlag_WithContainsArgument()
    {
        string[] args = ["--contains", "test"];

        Dictionary<FlagType, string> options = [];

        bool isSuccessful = Parse.TryParseOptions(args, out options);

        Assert.Equal(FlagType.Contains, options.Keys.First());
        Assert.True(isSuccessful);
    }

    [Fact]
    public void ParseArguments_ShouldReturnNoContainsFlag_WithNoContainsArgument()
    {
        string[] args = ["--no-contains", "test"];

        Dictionary<FlagType, string> options = [];

        bool isSuccessful = Parse.TryParseOptions(args, out options);

        Assert.Equal(FlagType.Nocontains, options.Keys.First());
        Assert.True(isSuccessful);
    }

    [Fact]
    public void ParseArguments_ShouldReturnError_WithInvalidArgument()
    {
        string[] args = ["--invalid"];

        Dictionary<FlagType, string> options = [];

        bool isSuccessful = Parse.TryParseOptions(args, out options);

        Assert.False(isSuccessful);
    }

    [Fact]
    public void ParseArguments_ShouldReturnError_WithDuplicateArgument()
    {
        string[] args = ["--contains", "test", "--contains", "test"];

        Dictionary<FlagType, string> options = [];

        bool isSuccessful = Parse.TryParseOptions(args, out options);

        Assert.False(isSuccessful);
    }

    [Fact]
    public void ParseArguments_ShouldReturnError_WithInvalidArgumentAfterFlag()
    {
        string[] args = ["--contains", "--invalid"];

        Dictionary<FlagType, string> options = [];

        bool isSuccessful = Parse.TryParseOptions(args, out options);

        Assert.False(isSuccessful);
    }

    [Fact]
    public void ParseArguments_ShouldReturnError_WithInvalidArgumentAfterFlagShort()
    {
        string[] args = ["-c", "--invalid"];

        Dictionary<FlagType, string> options = [];

        bool isSuccessful = Parse.TryParseOptions(args, out options);

        Assert.False(isSuccessful);
    }

    [Fact]
    public void ParseArguments_ShouldReturnError_WithInvalidArgumentAfterFlagShortAndValue()
    {
        string[] args = ["-c", "test", "--invalid"];

        Dictionary<FlagType, string> options = [];

        bool isSuccessful = Parse.TryParseOptions(args, out options);

        Assert.False(isSuccessful);
    }

    [Fact]
    public void ParseArguments_ShouldReturnError_WithInvalidArgumentAfterFlagShortAndValueShort()
    {
        string[] args = ["-c", "test", "-i"];

        Dictionary<FlagType, string> options = [];

        bool isSuccessful = Parse.TryParseOptions(args, out options);

        Assert.False(isSuccessful);
    }

    [Fact]
    public void ParseArguments_ShouldReturnError_WithInvalidArgumentAfterFlagShortAndValueShortAndValue()
    {
        string[] args = ["-c", "test", "-i", "test"];

        Dictionary<FlagType, string> options = [];

        bool isSuccessful = Parse.TryParseOptions(args, out options);

        Assert.False(isSuccessful);
    }

    [Fact]
    public void ParseArguments_ShouldReturnError_WithInvalidArgumentAfterFlagShortAndValueShortAndValueShort()
    {
        string[] args = ["-c", "test", "-i", "test", "-t"];

        Dictionary<FlagType, string> options = [];

        bool isSuccessful = Parse.TryParseOptions(args, out options);

        Assert.False(isSuccessful);
    }

    [Fact]
    public void ParseArguments_ShouldReturnError_WithInvalidArgumentAfterFlagShortAndValueShortAndValueShortAndValue()
    {
        string[] args = ["-c", "test", "-i", "test", "-t", "test"];

        Dictionary<FlagType, string> options = [];

        bool isSuccessful = Parse.TryParseOptions(args, out options);

        Assert.False(isSuccessful);
    }
}