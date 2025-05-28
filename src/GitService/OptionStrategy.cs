using Bbranch.Shared.TableData;

namespace Bbranch.GitService.OptionStrategies;

public interface IOption
{
    HashSet<GitBranch> Execute(HashSet<GitBranch> branches);
}

public sealed class CompositeOptionStrategy(List<IOption> options) : IOption
{
    public void AddStrategyOption(IOption option)
    {
        options.Add(option);
    }

    public HashSet<GitBranch> Execute(HashSet<GitBranch> branches)
    {
        return options.Aggregate(branches, (current, strategyOption) => strategyOption.Execute(current));
    }
}
