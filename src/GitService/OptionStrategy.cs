using System.Diagnostics;
using Bbranch.Shared.TableData;

namespace Bbranch.GitService.OptionStrategies;

public interface IOption
{
    Task<List<GitBranch>> Execute(List<GitBranch> branches);
}

public sealed class CompositeOptionStrategy : IOption
{
    private readonly List<IOption> _options;

    public CompositeOptionStrategy(List<IOption> options)
    {
        _options = options;
    }

    public IReadOnlyList<IOption> Options => _options;

    public void AddStrategyOption(IOption option)
    {
        _options.Add(option);
    }

    public async Task<List<GitBranch>> Execute(List<GitBranch> branches)
    {
        var result = branches;

        foreach (var option in _options)
        {
            var stopwatch = Stopwatch.StartNew();
            result = await option.Execute(result);

            stopwatch.Stop();
            Console.WriteLine($"{option.ToString()} took {stopwatch.ElapsedMilliseconds}ms");
        }

        return result;
    }
}
