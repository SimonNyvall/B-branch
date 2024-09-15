using Bbranch.Shared.TableData;

namespace Bbranch.GitService.OptionStrategies;

public interface IOption
{
    List<GitBranch> Execute(List<GitBranch> branches);
}

public class CompositeOptionStrategy : IOption
{
    private readonly List<IOption> _options;

    public CompositeOptionStrategy(List<IOption> options)
    {
        _options = options;
    }

    public void AddStrategyOption(IOption option)
    {
        _options.Add(option);
    }

    public List<GitBranch> Execute(List<GitBranch> branches)
    {
        foreach (IOption strategyOption in _options)
        {
            branches = strategyOption.Execute(branches);
        }

        return branches;
    }
}
