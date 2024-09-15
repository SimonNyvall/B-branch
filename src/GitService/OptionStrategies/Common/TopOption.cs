using Bbranch.Shared.TableData;

namespace Bbranch.GitService.OptionStrategies.Common;

public class TopOption(int takeYeild) : IOption
{
    public List<GitBranch> Execute(List<GitBranch> branches)
    {
        return branches.Take(takeYeild).ToList();
    }
}