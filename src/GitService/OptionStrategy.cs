using Bbranch.Shared.TableData;

namespace Bbranch.GitService.OptionStrategies;

public interface IOption
{
    List<GitBranch> Execute(List<GitBranch> branches);
}

public sealed class CompositeOptionStrategy(List<IOption> options) : IOption
{
    public void AddStrategyOption(IOption option)
    {
        options.Add(option);
    }

    public List<GitBranch> Execute(List<GitBranch> branches)
    {
        return options.Aggregate(branches, (current, strategyOption) => strategyOption.Execute(current));
    }
}
