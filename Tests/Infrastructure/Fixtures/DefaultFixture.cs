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

    public string PublishedArtifactPath => "./Cli";

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

    public Task<ExecResult> Exec(params string[] command)
    {
        return _container.ExecAsync(command);
    }

    public Task<ExecResult> Exec(string command)
    {
        return Exec("bash", "-c", command);
    }

    private async Task ExecOrThrow(string command)
    {
        Console.WriteLine();
        Console.WriteLine($">>> {command}");

        var result = await Exec(command);

        Console.WriteLine($"Exit Code: {result.ExitCode}");

        if (!string.IsNullOrWhiteSpace(result.Stdout))
        {
            Console.WriteLine("STDOUT:");
            Console.WriteLine(result.Stdout);
        }

        if (!string.IsNullOrWhiteSpace(result.Stderr))
        {
            Console.WriteLine("STDERR:");
            Console.WriteLine(result.Stderr);
        }

        if (result.ExitCode != 0)
        {
            throw new Exception(
                $"""
Command failed.

Command:
{command}

Exit Code:
{result.ExitCode}

Stdout:
{result.Stdout}

Stderr:
{result.Stderr}
"""
            );
        }
    }
}
