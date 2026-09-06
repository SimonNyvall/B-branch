using Bbranch.CLI.Output;
using Bbranch.GitService.GitRepositories;
using Bbranch.GitService.OptionStrategies;
using Bbranch.Shared.TableData;

namespace Bbranch.Cli.Assemblers;

public static class BranchAssembler
{
    public static async IAsyncEnumerable<List<GitBranch>> AssembleBranches(
        CompositeOptionStrategy compositeOptionStrategy,
        GitRepository gitRepository,
        BranchScope scope,
        int batchSize
    )
    {
        var branchBatches = GetBranches(gitRepository, scope, batchSize);

        await foreach (var batch in ProcessBatches(branchBatches, compositeOptionStrategy))
            yield return batch;
    }

    private static IAsyncEnumerable<List<GitBranch>> GetBranches(
        GitRepository gitRepository,
        BranchScope scope,
        int batchSize
    )
    {
        return scope switch
        {
            BranchScope.Local => gitRepository.InternalRepo.GetLocalBranchNames(batchSize),
            BranchScope.Remote => gitRepository.InternalRepo.GetRemoteBranchNames(batchSize),
            BranchScope.Both => Combine(
                gitRepository.InternalRepo.GetLocalBranchNames(batchSize / 2),
                gitRepository.InternalRepo.GetRemoteBranchNames(batchSize / 2)
            ),
            _ => throw new ArgumentException(""), // TODO: add some good message here
        };
    }

    private static async IAsyncEnumerable<List<GitBranch>> Combine(
        IAsyncEnumerable<List<GitBranch>> first,
        IAsyncEnumerable<List<GitBranch>> second
    )
    {
        await foreach (var batch in first)
        {
            yield return batch;
        }

        await foreach (var batch in second)
        {
            yield return batch;
        }
    }

    private static async IAsyncEnumerable<List<GitBranch>> ProcessBatches(
        IAsyncEnumerable<List<GitBranch>> branchBatches,
        CompositeOptionStrategy compositeOptionStrategy
    )
    {
        await foreach (var branches in branchBatches)
        {
            var result = await compositeOptionStrategy.Execute(branches);

            yield return result;
        }
    }
}
