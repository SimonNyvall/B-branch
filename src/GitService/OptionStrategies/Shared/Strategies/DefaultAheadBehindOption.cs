using Bbranch.GitService.Base;
using Bbranch.Shared.TableData;
using Git_Service.Base.Analizers;

namespace Bbranch.GitService.OptionStrategies.Shared.Strategies;

public sealed class DefaultAheadBehindOption(IGitRepository gitBase) : IOption
{
    public HashSet<GitBranch> Execute(HashSet<GitBranch> branches)
    {
        var options = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };
        var gitPath = gitBase.GitRepositoryPath;
        
        Parallel.ForEach(branches, options, branch =>
        {
            try
            {
                var aheadBehindFacade = new AheadBehindFacade(gitPath);

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
