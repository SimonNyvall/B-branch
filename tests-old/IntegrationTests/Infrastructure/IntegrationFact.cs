using System.Runtime.CompilerServices;

public class IntegrationFactAttribute : FactAttribute
{
    public IntegrationFactAttribute(
        [CallerFilePath] string? sourceFilePath = null,
        [CallerLineNumber] int sourceLineNumber = -1
    )
    {
        _ = sourceFilePath;
        _ = sourceLineNumber;

        if (Environment.GetEnvironmentVariable("RUN_INTEGRATION_TESTS") != "true")
        {
            Skip = "Integration tests require Docker. Set RUN_INTEGRATION_TESTS=true.";
        }
    }
}

public class IntegrationTheoryAttribute : TheoryAttribute
{
    public IntegrationTheoryAttribute(
        [CallerFilePath] string? sourceFilePath = null,
        [CallerLineNumber] int sourceLineNumber = -1
    )
    {
        _ = sourceFilePath;
        _ = sourceLineNumber;

        if (Environment.GetEnvironmentVariable("RUN_INTEGRATION_TESTS") != "true")
        {
            Skip = "Integration tests require Docker. Set RUN_INTEGRATION_TESTS=true.";
        }
    }
}
