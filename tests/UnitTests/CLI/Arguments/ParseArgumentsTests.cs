namespace Tests.Arguments;

using CLI.Flags;
using CLI.ParseArguments;

public class ParseArgumentsTests
{
    [Fact]
    public void ParseArguments_ShouldReturnVersionFlag_WithOnlyVersionArgument()
    {
        string[] args = ["--version"];

        Dictionary<FlagType, string> options = Parse.Arguments(args);

        Assert.Equal(FlagType.Version, options.Keys.First());
    }

    [Fact]
    public void ParseArguments_ShouldReturnContainsFlag_WithContainsArgument()
    {
        string[] args = ["--contains", "test"];

        Dictionary<FlagType, string> options = Parse.Arguments(args);

        Assert.Equal(FlagType.Contains, options.Keys.First());
    }

    [Fact]
    public void ParseArguments_ShouldReturnNoContainsFlag_WithNoContainsArgument()
    {
        string[] args = ["--no-contains", "test"];

        Dictionary<FlagType, string> options = Parse.Arguments(args);

        Assert.Equal(FlagType.Nocontains, options.Keys.First());
    }

    [Fact]
    public void ParseArguments_ShouldReturnError_WithInvalidArgument()
    {
        string[] args = ["--invalid"];

        Assert.Throws<ArgumentException>(() => Parse.Arguments(args));
    }

    [Fact]
    public void ParseArguments_ShouldReturnError_WithDuplicateArgument()
    {
        string[] args = ["--contains", "test", "--contains", "test"];

        Assert.Throws<ArgumentException>(() => Parse.Arguments(args));
    }

    [Fact]
    public void ParseArguments_ShouldReturnError_WithInvalidArgumentAfterFlag()
    {
        string[] args = ["--contains", "--invalid"];

        Assert.Throws<ArgumentException>(() => Parse.Arguments(args));
    }

    [Fact]
    public void ParseArguments_ShouldReturnError_WithInvalidArgumentAfterFlagShort()
    {
        string[] args = ["-c", "--invalid"];

        Assert.Throws<ArgumentException>(() => Parse.Arguments(args));
    }

    [Fact]
    public void ParseArguments_ShouldReturnError_WithInvalidArgumentAfterFlagShortAndValue()
    {
        string[] args = ["-c", "test", "--invalid"];

        Assert.Throws<ArgumentException>(() => Parse.Arguments(args));
    }

    [Fact]
    public void ParseArguments_ShouldReturnError_WithInvalidArgumentAfterFlagShortAndValueShort()
    {
        string[] args = ["-c", "test", "-i"];

        Assert.Throws<ArgumentException>(() => Parse.Arguments(args));
    }

    [Fact]
    public void ParseArguments_ShouldReturnError_WithInvalidArgumentAfterFlagShortAndValueShortAndValue()
    {
        string[] args = ["-c", "test", "-i", "test"];

        Assert.Throws<ArgumentException>(() => Parse.Arguments(args));
    }

    [Fact]
    public void ParseArguments_ShouldReturnError_WithInvalidArgumentAfterFlagShortAndValueShortAndValueShort()
    {
        string[] args = ["-c", "test", "-i", "test", "-t"];

        Assert.Throws<ArgumentException>(() => Parse.Arguments(args));
    }

    [Fact]
    public void ParseArguments_ShouldReturnError_WithInvalidArgumentAfterFlagShortAndValueShortAndValueShortAndValue()
    {
        string[] args = ["-c", "test", "-i", "test", "-t", "test"];

        Assert.Throws<ArgumentException>(() => Parse.Arguments(args));
    }
}