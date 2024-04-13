namespace tests;

using Bbranch.Flags;
using Bbranch.ValidateArguments;

public class ValidationTests
{
    [Fact]
    public void ValidateArguments_ShouldReturn_Success()
    {
        // Arrange
        Dictionary<string, string> options = [];

        // Act
        var result = Validate.Arguments(options);

        // Assert
        Assert.Equal(Result.Success, result);
    }
}
