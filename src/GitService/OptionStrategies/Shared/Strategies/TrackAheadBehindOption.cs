using Bbranch.Shared.TableData;
using Bbranch.GitService.Base;
using Bbranch.GitService.Base.Analyzers;

namespace Bbranch.GitService.OptionStrategies.Shared.Strategies;

public sealed class TrackAheadBehindOption : IOption
{
    private readonly IGitRepository _gitBase;
    private readonly string _remoteBranchName;
    private readonly AheadBehindFacade _aheadBehindFacade;
    private const int BatchSize = 50;

    public TrackAheadBehindOption(IGitRepository gitBase, string remoteBranchName)
    {
        _gitBase = gitBase;
        _remoteBranchName = remoteBranchName;
        _aheadBehindFacade = new AheadBehindFacade(_gitBase.GitRepositoryPath);
    }

    public HashSet<GitBranch> Execute(HashSet<GitBranch> branches)
    {
        var options = new ParallelOptions
        {
            MaxDegreeOfParallelism = Math.Min(Environment.ProcessorCount * 2, branches.Count)
        };

        Parallel.ForEach(branches, options, branch =>
        {
            try
            {
                var aheadBehind = _aheadBehindFacade.GetRemoteAheadBehind(branch.Branch.Name, _remoteBranchName)
                    .GetAwaiter()
                    .GetResult();
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