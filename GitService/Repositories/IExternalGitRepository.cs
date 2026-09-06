namespace Bbranch.GitService.GitRepositories;

public interface IExternalGitRepository
{
    Task<IReadOnlyDictionary<string, string>> GetMergeRequestUrls(IReadOnlySet<string> branchNames);
}
