using Shared.TableData;

namespace Git.Options;

public class TopOption(int takeYeild) : IOption
{
    public List<GitBranch> Execute(List<GitBranch> branches)
    {
        return branches.Take(takeYeild).ToList();
    }
}