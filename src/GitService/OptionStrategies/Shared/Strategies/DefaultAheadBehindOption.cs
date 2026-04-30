using Bbranch.GitService.Base;
using Bbranch.Shared.TableData;

namespace Bbranch.GitService.OptionStrategies.Shared.Strategies;

public sealed class DefaultAheadBehindOption(IGitRepository gitBase) : IOption
{
    public Task<HashSet<GitBranch>> Execute(HashSet<GitBranch> branches)
    {
        var options = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };

        Parallel.ForEach(
            branches,
            options,
            async branch =>
            {
                try
                {
                    var aheadBehind = await gitBase.GetLocalAheadBehind(branch.Branch.Name);
                    branch.SetAheadBehind(aheadBehind);
                }
                catch (Exception)
                {
                    branch.SetAheadBehind(new AheadBehind(0, 0));
                }
            }
        );

        return Task.FromResult(branches);
    }
}
