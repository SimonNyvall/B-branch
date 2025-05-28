using Bbranch.Shared.TableData;
using Bbranch.GitService.Base;
using MethodTimer;

namespace Bbranch.GitService.OptionStrategies.Shared.Strategies;

public sealed class TrackAheadBehindOption(IGitRepository gitBase, string remoteBranchName) : IOption
{
    public HashSet<GitBranch> Execute(HashSet<GitBranch> branches)
    {
        var options = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };
        
        Parallel.ForEach(branches, options, branch =>
        {
            try
            {
                var aheadBehind = gitBase.GetRemoteAheadBehind(branch.Branch.Name, remoteBranchName).GetAwaiter().GetResult();
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