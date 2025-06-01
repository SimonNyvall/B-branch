using Bbranch.GitService.Base;
using Bbranch.GitService.Base.Analyzers;
using Bbranch.Shared.TableData;

namespace Bbranch.GitService.OptionStrategies.Shared.Strategies;

public sealed class DefaultAheadBehindOption : IOption
{
    private readonly IGitRepository _gitBase;
    private readonly AheadBehindFacade _aheadBehindFacade;
    private const int BatchSize = 50;

    public DefaultAheadBehindOption(IGitRepository gitBase)
    {
        _gitBase = gitBase;
        _aheadBehindFacade = new AheadBehindFacade(_gitBase.GitRepositoryPath);
    }

    public HashSet<GitBranch> Execute(HashSet<GitBranch> branches)
    {
        var branchGroups = branches.Chunk(BatchSize);
        var options = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };

        foreach (var group in branchGroups)
        {
            Parallel.ForEach(group, options, branch =>
            {
                try
                {
                    var aheadBehind = _aheadBehindFacade.GetLocalAheadBehind(branch.Branch.Name)
                        .GetAwaiter()
                        .GetResult();
                    branch.SetAheadBehind(aheadBehind);
                }
                catch (Exception)
                {
                    branch.SetAheadBehind(new AheadBehind(0, 0));
                }
            });
        }

        Parallel.ForEach(branches, options, branch =>
        {
            try
            {
                var aheadBehindFacade = new AheadBehindFacade(_gitBase.GitRepositoryPath);

                var aheadBehind = aheadBehindFacade.GetLocalAheadBehind(branch.Branch.Name).GetAwaiter().GetResult();
                branch.SetAheadBehind(aheadBehind);
            }
            catch (Exception)
            {
                branch.SetAheadBehind(new AheadBehind(0, 0));
            }
        });

        return branches;
    }
}
