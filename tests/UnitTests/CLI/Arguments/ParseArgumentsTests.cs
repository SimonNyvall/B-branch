using Bbranch.CLI.Arguments;
using Bbranch.CLI.Arguments.FlagSystem;
using Bbranch.CLI.Arguments.FlagSystem.Flags;

namespace Bbranch.Tests.CLI.Arguments;

public class ParseArgumentsTests
{
    [Fact]
    public void Given_Parse_When_ParseArgumentsRun_Then_Return_VersionFlag_WithOnlyVersionArgument()
    {
        string[] args = ["--version"];

        FlagCollection options = [];

        bool isSuccessful = Parse.TryParseOptions(args, out options);

        Assert.True(options.Contains<VersionFlag>());
        Assert.True(isSuccessful);
    }

    [Fact]
    public void Given_Parse_When_ParseArgumentsRun_Then_Return_ContainsFlag_WithContainsArgument()
    {
        string[] args = ["--contains", "test"];

        FlagCollection options = [];

        bool isSuccessful = Parse.TryParseOptions(args, out options);

        Assert.True(options.Contains<ContainsFlag>());
        Assert.True(isSuccessful);
    }

    [Fact]
    public void Given_Parse_When_ParseArgumentsRun_Then_Return_NoContainsFlag_WithNoContainsArgument()
    {
        string[] args = ["--no-contains", "test"];

        FlagCollection options = [];

        bool isSuccessful = Parse.TryParseOptions(args, out options);

        Assert.True(options.Contains<NoContainsFlag>());
        Assert.True(isSuccessful);
    }

    [Fact]
    public void Given_Parse_When_ParseArgumentsRun_Then_Return_Error_WithInvalidArgument()
    {
        string[] args = ["--invalid"];

        FlagCollection options = [];

        bool isSuccessful = Parse.TryParseOptions(args, out options);

        Assert.False(isSuccessful);
    }

    [Fact]
    public void Given_Parse_When_ParseArgumentsRun_Then_Return_Error_WithDuplicateArgument()
    {
        string[] args = ["--contains", "test", "--contains", "test"];

        FlagCollection options = [];

        bool isSuccessful = Parse.TryParseOptions(args, out options);

        Assert.False(isSuccessful);
    }

    [Fact]
    public void Given_Parse_When_ParseArgumentsRun_Then_Return_Error_WithInvalidArgumentAfterFlag()
    {
        string[] args = ["--contains", "--invalid"];

        FlagCollection options = [];

        bool isSuccessful = Parse.TryParseOptions(args, out options);

        Assert.False(isSuccessful);
    }

    [Fact]
    public void Given_Parse_When_ParseArgumentsRun_Then_Return_Error_WithInvalidArgumentAfterFlagShort()
    {
        string[] args = ["-c", "--invalid"];

        FlagCollection options = [];

        bool isSuccessful = Parse.TryParseOptions(args, out options);

        Assert.False(isSuccessful);
    }

    [Fact]
    public void Given_Parse_When_ParseArgumentsRun_Then_Return_Error_WithInvalidArgumentAfterFlagShortAndValue()
    {
        string[] args = ["-c", "test", "--invalid"];

        FlagCollection options = [];

        bool isSuccessful = Parse.TryParseOptions(args, out options);

        Assert.False(isSuccessful);
    }

    [Fact]
    public void Given_Parse_When_ParseArgumentsRun_Then_Return_Error_WithInvalidArgumentAfterFlagShortAndValueShort()
    {
        string[] args = ["-c", "test", "-i"];

        FlagCollection options = [];

        bool isSuccessful = Parse.TryParseOptions(args, out options);

        Assert.False(isSuccessful);
    }

    [Fact]
    public void Given_Parse_When_ParseArgumentsRun_Then_Return_Error_WithInvalidArgumentAfterFlagShortAndValueShortAndValue()
    {
        string[] args = ["-c", "test", "-i", "test"];

        FlagCollection options = [];

        bool isSuccessful = Parse.TryParseOptions(args, out options);

        Assert.False(isSuccessful);
    }

    [Fact]
    public void Given_Parse_When_ParseArgumentsRun_Then_Return_Error_WithInvalidArgumentAfterFlagShortAndValueShortAndValueShort()
    {
        string[] args = ["-c", "test", "-i", "test", "-t"];

        FlagCollection options = [];

        bool isSuccessful = Parse.TryParseOptions(args, out options);

        Assert.False(isSuccessful);
    }

    [Fact]
    public void Given_Parse_When_ParseArgumentsRun_Then_Return_Error_WithInvalidArgumentAfterFlagShortAndValueShortAndValueShortAndValue()
    {
        string[] args = ["-c", "test", "-i", "test", "-t", "test"];

        FlagCollection options = [];

        bool isSuccessful = Parse.TryParseOptions(args, out options);

        Assert.False(isSuccessful);
    }

    [Fact]
    public void Given_Parse_When_ParseArgumentsRun_Then_Return_HelpFlag_WithOnlyHelpArgument()
    {
        string[] args = ["--help"];

        FlagCollection options = [];

        bool isSuccessful = Parse.TryParseOptions(args, out options);

        Assert.True(options.Contains<HelpFlag>());
        Assert.True(isSuccessful);
    }

    [Fact]
    public void Given_Parse_When_ParseArgumentsRun_Then_Return_QuietFlagAndAllFlag_WithQuietAndAllConcatenatedArguments()
    {
        string[] args = ["-qa"];

        FlagCollection options = [];

        bool isSuccessful = Parse.TryParseOptions(args, out options);

        Assert.True(options.Contains<quietFlag>());
        Assert.True(options.Contains<AllFlag>());
        Assert.True(isSuccessful);
        Assert.Equal(2, options.Count);
    }

    [Fact]
    public void Given_Parse_When_ParseArgumentsRun_Then_Return_QuietFlagAndAllFlagAndSortFlag_WithQuietAndAllAndSortConcatenatedArguments()
    {
        string[] args = ["-qa", "--sort", "name"];

        FlagCollection options = [];

        bool isSuccessful = Parse.TryParseOptions(args, out options);

        Assert.True(options.Contains<quietFlag>());
        Assert.True(options.Contains<AllFlag>());
        Assert.True(options.Contains<SortFlag>());
        Assert.True(isSuccessful);
        Assert.Equal(3, options.Count);
    }

    [Fact]
    public void Given_Parse_When_ParseArgumentsRun_Then_Return_Error_WithConcatFlagAndInvalidDashCount()
    {
        string[] args = ["--qa"];

        FlagCollection options = [];

        bool isSuccessful = Parse.TryParseOptions(args, out options);

        Assert.False(isSuccessful);
    }

    [Fact]
    public void Given_Parse_When_ParseArgumentsRun_Then_Return_Error_WithDuplicatedConcatFlag()
    {
        string[] args = ["-qaq"];

        FlagCollection options = [];

        bool isSuccessful = Parse.TryParseOptions(args, out options);

        Assert.False(isSuccessful);
    }
}