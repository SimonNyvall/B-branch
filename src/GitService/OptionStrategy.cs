using Bbranch.Shared.TableData;

namespace Bbranch.GitService.OptionStrategies;

public interface IOption
{
    Task<HashSet<GitBranch>> Execute(HashSet<GitBranch> branches);
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

    public async Task<HashSet<GitBranch>> Execute(HashSet<GitBranch> branches)
    {
        var result = branches;

        foreach (var option in _options)
        {
            result = await option.Execute(result);
        }

        return result;
    }
}
