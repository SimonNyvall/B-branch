using Bbranch.GitService.GitRepositories;
using Bbranch.Shared.TableData;

namespace Bbranch.GitService.OptionStrategies.Common;

public sealed class StatusOption(GitRepository gitRepository) : IOption
{
    public async Task<List<GitBranch>> Execute(List<GitBranch> branches)
    {
        var now = DateTime.Now;

        var branchNames = branches
            .Select(x => x.Branch.Name)
            .Where(x => !x.StartsWith("origin/HEAD"))
            .Select(x => x.StartsWith("origin/") ? x["origin/".Length..] : x)
            .ToHashSet();

        var mergeRequestUrls = await gitRepository.ExternalRepo.GetMergeRequestUrls(branchNames);

        foreach (var branch in branches)
        {
            var branchName = branch.Branch.Name;
            if (branchName.StartsWith("origin/", StringComparison.Ordinal))
            {
                branchName = branchName["origin/".Length..];
            }

            if (branchName == "main" || branchName == "main" || branch.IsSymbolic)
            {
                continue;
            }

            if (gitRepository.InternalRepo.IsBranchMerged(branchName))
            {
                branch.SetStatus(Status.Merged);

                if (mergeRequestUrls.TryGetValue(branchName, out var url))
                {
                    branch.SetMergeRequestUrl(url);
                }

                continue;
            }

            var status = (now - branch.LastCommit).Days switch
            {
                >= 0 and < 30 => Status.Active,
                >= 30 and < 60 => Status.Inactive,
                >= 60 and < 90 => Status.Stale,
                _ => Status.Abandoned,
            };

            branch.SetStatus(status);
        }

        return branches;
    }
}
