using System.Runtime.CompilerServices;
using Bbranch.Tests.Infrastucture;

namespace BBranch.Tests.Integration.Infrastructure;

public class DockerFactAttribute : FactAttribute
{
    public DockerFactAttribute(
        [CallerFilePath] string? sourceFilePath = null,
        [CallerLineNumber] int sourceLineNumber = -1
    )
    {
        _ = sourceFilePath;
        _ = sourceLineNumber;

        if (!DockerContext.IsDockerRunning)
        {
            Skip = "Integration tests require Docker.";
        }
    }
}

public class DockerTheoryAttribute : TheoryAttribute
{
    public DockerTheoryAttribute(
        [CallerFilePath] string? sourceFilePath = null,
        [CallerLineNumber] int sourceLineNumber = -1
    )
    {
        _ = sourceFilePath;
        _ = sourceLineNumber;

        if (!DockerContext.IsDockerRunning)
        {
            Skip = "Integration tests require Docker.";
        }
    }
}
