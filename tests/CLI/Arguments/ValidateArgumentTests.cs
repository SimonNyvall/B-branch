namespace Tests.Arguments;

using CLI.Flags;
using CLI.ValidateArguments;

public class ValidateArgumentTests
{
    [Fact]
    public void ValidateArguments_ShouldReturnSuccess_WithOnlyVersionArgument()
    {
        var options = new Dictionary<FlagType, string>
        {
            { FlagType.Version, string.Empty }
        };

        Result result = Validate.Arguments(options);

        Assert.Equal(Result.Success, result);
    }

    [Fact]
    public void ValidateArguments_ShouldRetrunError_WithVersionAndContainsArguments()
    {
        var options = new Dictionary<FlagType, string>
        {
            { FlagType.Version, string.Empty },
            { FlagType.Contains, string.Empty }
        };

        Result result = Validate.Arguments(options);

        Assert.Equal(Result.Error, result);
    }

    [Fact]
    public void ValidateArguments_ShouldReturnSuccess_WithContainsArgument()
    {
        var options = new Dictionary<FlagType, string>
        {
            { FlagType.Contains, string.Empty }
        };

        Result result = Validate.Arguments(options);

        Assert.Equal(Result.Success, result);
    }

    [Fact]
    public void ValidateArguments_ShouldReturnError_WithContainsAndNoContainsArguments()
    {
        var options = new Dictionary<FlagType, string>
        {
            { FlagType.Contains, string.Empty },
            { FlagType.Nocontains, string.Empty }
        };

        Result result = Validate.Arguments(options);

        Assert.Equal(Result.Error, result);
    }

    [Fact]
    public void ValidateArguments_ShouldReturnSuccess_WithAllArgument()
    {
        var options = new Dictionary<FlagType, string>
        {
            { FlagType.All, string.Empty }
        };

        Result result = Validate.Arguments(options);

        Assert.Equal(Result.Success, result);
    }

    [Fact]
    public void ValidateArguments_ShouldReturnSuccess_WithRemoteArgument()
    {
        var options = new Dictionary<FlagType, string>
        {
            { FlagType.Remote, string.Empty }
        };

        Result result = Validate.Arguments(options);

        Assert.Equal(Result.Success, result);
    }

    [Fact]
    public void ValidateArguments_ShouldReturnError_WithAllAndRemoteArguments()
    {
        var options = new Dictionary<FlagType, string>
        {
            { FlagType.All, string.Empty },
            { FlagType.Remote, string.Empty }
        };

        Result result = Validate.Arguments(options);

        Assert.Equal(Result.Error, result);
    }

    [Fact]
    public void ValidateArguments_ShoudlReturnSuccess_WithSortDateValueArgument()
    {
        var options = new Dictionary<FlagType, string>
        {
            { FlagType.Sort, "date" }
        };

        Result result = Validate.Arguments(options);

        Assert.Equal(Result.Success, result);
    }

    [Fact]
    public void ValidateArguments_ShouldReturnSuccess_WithSortNameValueArgument()
    {
        var options = new Dictionary<FlagType, string>
        {
            { FlagType.Sort, "name" }
        };

        Result result = Validate.Arguments(options);

        Assert.Equal(Result.Success, result);
    }

    [Fact]
    public void ValidateArguments_ShouldReturnSuccess_WithSortAheadValueArgument()
    {
        var options = new Dictionary<FlagType, string>
        {
            { FlagType.Sort, "ahead" }
        };

        Result result = Validate.Arguments(options);

        Assert.Equal(Result.Success, result);
    }

    [Fact]
    public void ValidateArguments_ShouldReturnSuccess_WithSortBehindValueArgument()
    {
        var options = new Dictionary<FlagType, string>
        {
            { FlagType.Sort, "behind" }
        };

        Result result = Validate.Arguments(options);

        Assert.Equal(Result.Success, result);
    }

    [Fact]
    public void ValidateArguments_ShouldReturnError_WithSortInvalidValueArgument()
    {
        var options = new Dictionary<FlagType, string>
        {
            { FlagType.Sort, "invalid" }
        };

        Result result = Validate.Arguments(options);

        Assert.Equal(Result.Error, result);
    }

    [Fact]
    public void ValidateArguments_ShouldReturnSucccess_WithPrintTopValueArgument()
    {
        var options = new Dictionary<FlagType, string>
        {
            { FlagType.Printtop, "5" }
        };

        Result result = Validate.Arguments(options);

        Assert.Equal(Result.Success, result);
    }

    [Fact]
    public void ValidateArguments_ShouldReturnError_WithPrintTopInvalidValueArgument()
    {
        var options = new Dictionary<FlagType, string>
        {
            { FlagType.Printtop, "invalid" }
        };

        Result result = Validate.Arguments(options);

        Assert.Equal(Result.Error, result);
    }

    [Fact]
    public void ValidateArguments_ShouldReturnError_WithPrintTopNegativeValueArgument()
    {
        var options = new Dictionary<FlagType, string>
        {
            { FlagType.Printtop, "-5" }
        };

        Result result = Validate.Arguments(options);

        Assert.Equal(Result.Error, result);
    }
}