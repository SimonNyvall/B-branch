using System.Text.RegularExpressions;
using Bbranch.Tests.Infrastucture.Fixtures;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Images;

[assembly: AssemblyFixture(typeof(DefaultFixture))]

namespace Bbranch.Tests.Infrastucture.Fixtures;

public sealed class DefaultFixture : IAsyncLifetime
{
    private IContainer _container = null!;
    private readonly IFutureDockerImage _image;

    private static string PublishedArtifactPath => "./Cli";

    public DefaultFixture()
    {
        var solutionRoot = Path.GetFullPath(
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..")
        );

        _image = new ImageFromDockerfileBuilder()
            .WithDockerfileDirectory(solutionRoot)
            .WithDockerfile("Dockerfile")
            .Build();
    }

    public async ValueTask InitializeAsync()
    {
        await _image.CreateAsync();

        _container = new ContainerBuilder(_image).WithCommand("tail", "-f", "/dev/null").Build();

        await _container.StartAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await _container.DisposeAsync();
    }

    public async Task<string> Exec(params string[] requestedCommands)
    {
        var executionCommands = new List<string> { DefaultFixture.PublishedArtifactPath };

        executionCommands.AddRange(requestedCommands);

        var execResult = await _container.ExecAsync(executionCommands);
        var result = execResult.Stdout.Replace("The less command was not found\n\n", "");
        result = Regex.Replace(result, @"\x1B\[[0-9;]*[A-Za-z]", "");
        return result.Replace("\r", "");
    }
}
