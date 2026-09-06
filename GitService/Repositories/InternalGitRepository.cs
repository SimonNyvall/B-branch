using Bbranch.GitService.Base.Commands;
using Bbranch.Shared.TableData;
using LibGit2Sharp;

namespace Bbranch.GitService.GitRepositories;

public sealed class InternalGitRepository : IInternalGitRepository
{
    private const int MaxCacheSize = 1000;
    private const int DecompressionBufferSize = 4096;
    internal static IIOAbstration _iOAbstration = null!;

    internal readonly string _gitPath;
    internal readonly bool _isWorktreeRepo;

    private string _headContent = string.Empty;

    public InternalGitRepository(IIOAbstration iOAbstration, string gitPath, bool isWorkTree)
    {
        _iOAbstration = iOAbstration;
        _gitPath = gitPath;
        _isWorktreeRepo = isWorkTree;
    }

    public async Task<string> GetWorkingBranch()
    {
        string headFileContent = string.Empty;

        if (string.IsNullOrEmpty(_headContent))
        {
            string headFilePath = Path.Combine(_gitPath, "HEAD");
            headFileContent = (await _iOAbstration.ReadAllText(headFilePath)).Trim();
        }
        else
        {
            headFileContent = _headContent;
        }

        if (headFileContent.StartsWith("ref:", StringComparison.Ordinal))
        {
            const int branchNameIndex = 2;
            string[] branchNameComponents = headFileContent.Split('/');

            string currentWorkingBranchName = string.Join(
                "/",
                branchNameComponents.Skip(branchNameIndex)
            );
            return currentWorkingBranchName;
        }

        return string.Empty;
    }

    public async Task<GitBranch> GetLastCommitDate(GitBranch branch)
    {
        var commitPath = branch.IsRemote ? "remotes" : "heads";
        var refPath = Path.Combine(_gitPath, "refs", commitPath, branch.Branch.Name);

        if (!_iOAbstration.FileExists(refPath))
        {
            return RunLastCommitDataFetchCommand(branch);
        }

        string commitHash = (await _iOAbstration.ReadAllText(refPath)).Trim();

        if (string.IsNullOrWhiteSpace(commitHash) || commitHash.Length < 3)
        {
            return RunLastCommitDataFetchCommand(branch);
        }

        string dirName = commitHash[..2];
        string objectName = commitHash[2..];

        var objectDir = Path.Combine(_gitPath, "objects", dirName);

        if (!_iOAbstration.DirectoryExists(objectDir))
        {
            return RunLastCommitDataFetchCommand(branch);
        }

        var objectPath = Path.Combine(objectDir, objectName);

        if (!_iOAbstration.FileExists(objectPath))
        {
            return RunLastCommitDataFetchCommand(branch);
        }

        DateTime lastWriteTime = _iOAbstration.GetLastWriteTime(objectPath);

        if (lastWriteTime.Year <= 1601)
        {
            return RunLastCommitDataFetchCommand(branch);
        }

        return branch.SetLastCommit(lastWriteTime);
    }

    private GitBranch RunLastCommitDataFetchCommand(GitBranch branch)
    {
        var branchName = branch.IsSymbolic ? branch.Branch.Name.Split(' ')[^1] : branch.Branch.Name;

        var lastWriteTime = new LastCommitDateFetchCommand(branchName).Execute();
        return branch.SetLastCommit(lastWriteTime);
    }

    public async Task<GitBranch> GetRemoteAheadBehind(
        GitBranch localBranch,
        string remoteBranchName
    )
    {
        try
        {
            return await GetAheadBehind(localBranch, remoteBranchName);
        }
        catch (Exception)
        {
            return localBranch.SetAheadBehind(new AheadBehind(0, 0));
        }
    }

    public async Task<GitBranch> GetLocalAheadBehind(GitBranch localBranch)
    {
        try
        {
            return await GetAheadBehind(localBranch);
        }
        catch
        {
            return localBranch.SetAheadBehind(new AheadBehind(0, 0));
        }
    }

    private async Task<GitBranch> GetAheadBehind(
        GitBranch localGitBranch,
        string? remoteBranchName = null
    )
    {
        using var repo = new Repository(_gitPath);

        var local = repo.Branches[localGitBranch.Branch.Name];

        var remote = remoteBranchName is null
            ? repo.Branches[$"origin/{localGitBranch.Branch.Name}"]
            : repo.Branches[$"origin/{remoteBranchName}"];

        if (local is null || remote is null)
        {
            return localGitBranch.SetAheadBehind(new AheadBehind(0, 0));
        }

        var divergence = repo.ObjectDatabase.CalculateHistoryDivergence(local.Tip, remote.Tip);

        var aheadBehind = new AheadBehind(divergence.AheadBy ?? 0, divergence.BehindBy ?? 0);
        return localGitBranch.SetAheadBehind(aheadBehind);
    }

    public async IAsyncEnumerable<List<GitBranch>> GetLocalBranchNames(int batchSize)
    {
        using var repository = new Repository(_gitPath);

        var batch = new List<GitBranch>(batchSize);

        foreach (var branch in repository.Branches.Where(b => !b.IsRemote))
        {
            var gitBranch = GitBranch
                .Default()
                .SetBranch(new BranchViewModel(branch.FriendlyName, false));

            batch.Add(gitBranch);

            if (batch.Count < batchSize)
                continue;

            yield return batch;
            batch = new List<GitBranch>(batchSize);
        }

        var detachedHead = await GetDetachedHead();

        if (detachedHead != null)
        {
            batch.Add(detachedHead);

            if (batch.Count >= batchSize)
            {
                yield return batch;
                batch = new List<GitBranch>(batchSize);
            }
        }

        if (batch.Count > 0)
            yield return batch;
    }

    public async IAsyncEnumerable<List<GitBranch>> GetRemoteBranchNames(int batchSize)
    {
        using var repository = new Repository(_gitPath);

        var batch = new List<GitBranch>(batchSize);
        var remotes = repository.Network.Remotes;

        foreach (var remote in remotes)
        {
            var remoteHead = await GetRemoteHead(remote.Name);

            if (remoteHead == null)
                continue;

            batch.Add(remoteHead);

            if (batch.Count < batchSize)
                continue;

            yield return batch;
            batch = new List<GitBranch>(batchSize);
        }

        foreach (
            var branch in repository.Branches.Where(b =>
                b.IsRemote
                && !b.FriendlyName.EndsWith("/HEAD", StringComparison.CurrentCultureIgnoreCase)
            )
        )
        {
            var gitBranch = GitBranch
                .Default()
                .SetBranch(new BranchViewModel(branch.FriendlyName, false))
                .SetIsRemote(true);

            batch.Add(gitBranch);

            if (batch.Count >= batchSize)
            {
                yield return batch;
                batch = new List<GitBranch>(batchSize);
            }
        }

        if (batch.Count > 0)
            yield return batch;
    }

    private async Task<GitBranch?> GetRemoteHead(string remoteName)
    {
        var headPath = Path.Combine(_gitPath, "refs", "remotes", remoteName, "HEAD");

        if (!_iOAbstration.FileExists(headPath))
            return null;

        var content = await _iOAbstration.ReadAllText(headPath);

        if (!content.StartsWith("ref: "))
            return null;

        var target = content.Substring(5).Trim();
        var shortTarget = target.Replace("refs/remotes/", "");

        var displayName = $"{remoteName}/HEAD -> {shortTarget}";

        BranchViewModel branch = new(displayName, false);

        return GitBranch
            .Default()
            .SetBranch(branch)
            .SetIsRemote(true)
            .SetSymLink(new Symbolic(remoteName, shortTarget));
    }

    private async Task<GitBranch?> GetDetachedHead()
    {
        var headPath = Path.Combine(_gitPath, "HEAD");
        var headContent = await _iOAbstration.ReadAllText(headPath);
        _headContent = headContent;

        if (headContent.StartsWith("ref: "))
        {
            return null;
        }

        var commitHash = headContent[..7];

        var branchName = $"(HEAD detached at {commitHash})";

        var branch = new BranchViewModel(branchName, true);

        return GitBranch.Default().SetBranch(branch).SetDetachedHead(commitHash);
    }

    public async Task<List<GitBranch>> GetBranchDescription(List<GitBranch> branches)
    {
        const string descriptionFileName = "EDIT_DESCRIPTION";
        string path = Path.Combine(_gitPath, descriptionFileName);

        if (!_iOAbstration.FileExists(path))
            return branches;

        var lines = await _iOAbstration.ReadAllLines(path);

        var descriptions = new Dictionary<string, string>();
        string? currentBranch = null;
        var currentDescription = new List<string>();

        foreach (var rawLine in lines)
        {
            var line = rawLine.Trim();

            if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                continue;

            if (line.StartsWith("[") && line.EndsWith("]"))
            {
                if (currentBranch != null)
                {
                    descriptions[currentBranch] = string.Join(" ", currentDescription).Trim();
                }

                currentBranch = line[1..^1];
                currentDescription.Clear();
                continue;
            }

            if (currentBranch != null)
            {
                currentDescription.Add(line);
            }
        }

        if (currentBranch != null)
        {
            descriptions[currentBranch] = string.Join(" ", currentDescription).Trim();
        }

        foreach (var branch in branches)
        {
            if (descriptions.TryGetValue(branch.Branch.Name, out var desc))
            {
                branch.SetDescription(desc);
            }
        }

        return branches;
    }

    public List<GitBranch> StichWorkTreeBranches(List<GitBranch> branches)
    {
        if (!_isWorktreeRepo)
        {
            return branches;
        }

        var worktreePath = Path.Combine(_gitPath, "worktrees");

        var checkedOutBrancheNames = new List<string>();

        foreach (var worktreeDirectory in _iOAbstration.GetDirectories(worktreePath))
        {
            checkedOutBrancheNames.Add(worktreeDirectory.Split(Path.DirectorySeparatorChar)[^1]);
        }

        foreach (var branch in branches)
        {
            var worktreeBranch = checkedOutBrancheNames.FirstOrDefault(x =>
                branch.Branch.Name.Equals(x, StringComparison.CurrentCulture)
            );

            if (worktreeBranch == null)
            {
                continue;
            }

            branch.SetIsCheckoutWorktree(true);
        }

        return branches;
    }

    public bool IsBranchMerged(string branchName)
    {
        using var repo = new Repository(_gitPath);

        var feature = repo.Branches[branchName];
        var main = repo.Branches["main"];

        if (feature?.Tip == null || main?.Tip == null)
            return false;

        // Case 1: Normal merge / fast-forward
        var mergeBase = repo.ObjectDatabase.FindMergeBase(feature.Tip, main.Tip);

        if (mergeBase?.Sha == feature.Tip.Sha)
            return true;

        // Case 2: Squash merge
        return IsSquashMerged(repo, feature.Tip, main.Tip, mergeBase);
    }

    private bool IsSquashMerged(
        Repository repo,
        Commit featureTip,
        Commit mainTip,
        Commit? mergeBase
    )
    {
        if (mergeBase == null)
            return false;

        // Get the changes introduced by the feature branch.
        var featurePatch = repo.Diff.Compare<Patch>(mergeBase.Tree, featureTip.Tree);

        if (!featurePatch.Any())
            return true;

        // Compare those changes against main.
        var mainPatch = repo.Diff.Compare<Patch>(mergeBase.Tree, mainTip.Tree);

        // Every file change from the feature branch should be
        // represented in the changes that exist on main.
        foreach (var featureChange in featurePatch)
        {
            var mainChange = mainPatch.FirstOrDefault(x => x.Path == featureChange.Path);

            if (mainChange == null)
                return false;

            if (mainChange.Patch != featureChange.Patch)
                return false;
        }

        return true;
    }
}
