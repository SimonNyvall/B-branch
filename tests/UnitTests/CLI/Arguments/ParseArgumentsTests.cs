using Bbranch.CLI.Arguments;
using Bbranch.CLI.Arguments.FlagSystem;
using Bbranch.CLI.Arguments.FlagSystem.Flags;

namespace Bbranch.Tests.CLI.Arguments;

public class ParseArgumentsTests
{
    [Fact]
    public void ParseArguments_ShouldReturnVersionFlag_WithOnlyVersionArgument()
    {
        string[] args = ["--version"];

        FlagCollection options = [];

        bool isSuccessful = Parse.TryParseOptions(args, out options);

        Assert.True(options.Contains<VersionFlag>());
        Assert.True(isSuccessful);
    }

    [Fact]
    public void ParseArguments_ShouldReturnContainsFlag_WithContainsArgument()
    {
        string[] args = ["--contains", "test"];

        FlagCollection options = [];

        bool isSuccessful = Parse.TryParseOptions(args, out options);

        Assert.True(options.Contains<ContainsFlag>());
        Assert.True(isSuccessful);
    }

    [Fact]
    public void ParseArguments_ShouldReturnNoContainsFlag_WithNoContainsArgument()
    {
        string[] args = ["--no-contains", "test"];

        FlagCollection options = [];

        bool isSuccessful = Parse.TryParseOptions(args, out options);

        Assert.True(options.Contains<NoContainsFlag>());
        Assert.True(isSuccessful);
    }

    [Fact]
    public void ParseArguments_ShouldReturnError_WithInvalidArgument()
    {
        string[] args = ["--invalid"];

        FlagCollection options = [];

        bool isSuccessful = Parse.TryParseOptions(args, out options);

        Assert.False(isSuccessful);
    }

    [Fact]
    public void ParseArguments_ShouldReturnError_WithDuplicateArgument()
    {
        string[] args = ["--contains", "test", "--contains", "test"];

        FlagCollection options = [];

        bool isSuccessful = Parse.TryParseOptions(args, out options);

        Assert.False(isSuccessful);
    }

    [Fact]
    public void ParseArguments_ShouldReturnError_WithInvalidArgumentAfterFlag()
    {
        string[] args = ["--contains", "--invalid"];

        FlagCollection options = [];

        bool isSuccessful = Parse.TryParseOptions(args, out options);

        Assert.False(isSuccessful);
    }

    [Fact]
    public void ParseArguments_ShouldReturnError_WithInvalidArgumentAfterFlagShort()
    {
        string[] args = ["-c", "--invalid"];

        FlagCollection options = [];

        bool isSuccessful = Parse.TryParseOptions(args, out options);

        Assert.False(isSuccessful);
    }

    [Fact]
    public void ParseArguments_ShouldReturnError_WithInvalidArgumentAfterFlagShortAndValue()
    {
        string[] args = ["-c", "test", "--invalid"];

        FlagCollection options = [];

        bool isSuccessful = Parse.TryParseOptions(args, out options);

        Assert.False(isSuccessful);
    }

    [Fact]
    public void ParseArguments_ShouldReturnError_WithInvalidArgumentAfterFlagShortAndValueShort()
    {
        string[] args = ["-c", "test", "-i"];

        FlagCollection options = [];

        bool isSuccessful = Parse.TryParseOptions(args, out options);

        Assert.False(isSuccessful);
    }

    [Fact]
    public void ParseArguments_ShouldReturnError_WithInvalidArgumentAfterFlagShortAndValueShortAndValue()
    {
        string[] args = ["-c", "test", "-i", "test"];

        FlagCollection options = [];

        bool isSuccessful = Parse.TryParseOptions(args, out options);

        Assert.False(isSuccessful);
    }

    [Fact]
    public void ParseArguments_ShouldReturnError_WithInvalidArgumentAfterFlagShortAndValueShortAndValueShort()
    {
        string[] args = ["-c", "test", "-i", "test", "-t"];

        FlagCollection options = [];

        bool isSuccessful = Parse.TryParseOptions(args, out options);

        Assert.False(isSuccessful);
    }

    [Fact]
    public void ParseArguments_ShouldReturnError_WithInvalidArgumentAfterFlagShortAndValueShortAndValueShortAndValue()
    {
        string[] args = ["-c", "test", "-i", "test", "-t", "test"];

        FlagCollection options = [];

        bool isSuccessful = Parse.TryParseOptions(args, out options);

        Assert.False(isSuccessful);
    }

    [Fact]
    public void ParseArgument_ShouldReturnHelpFlag_WithOnlyHelpArgument()
    {
        string[] args = ["--help"];

        FlagCollection options = [];

        bool isSuccessful = Parse.TryParseOptions(args, out options);

        Assert.True(options.Contains<HelpFlag>());
        Assert.True(isSuccessful);
    }
}