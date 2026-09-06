namespace Bbranch.GitService.GitRepositories;

public class GitRepository
{
    public string GitPath { get; private set; } = string.Empty;
    public bool IsWorktree { get; private set; }
    public IInternalGitRepository InternalRepo { get; private set; }
    public IExternalGitRepository ExternalRepo { get; private set; }
    private static GitRepository? _instance;
    private static IIOAbstration _iOAbstration = null!;

    private GitRepository(IIOAbstration iOAbstration, string gitPath, bool isWorkTree)
    {
        _iOAbstration = iOAbstration;
        GitPath = gitPath;
        IsWorktree = isWorkTree;
        InternalRepo = new InternalGitRepository(iOAbstration, gitPath, isWorkTree);

        var httpClient = new HttpClient();
        ExternalRepo = new ExternalGitRepository(httpClient, gitPath);
    }

    public static async Task<GitRepository> GetInstance(IIOAbstration? iOAbstration = null)
    {
        iOAbstration ??= new IOAbstraction();
        var (gitPath, isWorkTree) = await SetGitPath(iOAbstration);

        _instance ??= new GitRepository(iOAbstration, gitPath, isWorkTree);

        return _instance;
    }

    private static async Task<(string GitPath, bool IsWorkTree)> SetGitPath(
        IIOAbstration iOAbstration
    )
    {
        var dir = iOAbstration.GetCurrentDirectory();

        while (dir != null)
        {
            var dotGit = Path.Combine(dir.FullName, ".git");

            if (iOAbstration.DirectoryExists(dotGit))
            {
                return (dotGit, false);
            }

            if (iOAbstration.FileExists(dotGit))
            {
                var content = (await iOAbstration.ReadAllText(dotGit)).Trim();

                if (content.StartsWith("gitdir:", StringComparison.OrdinalIgnoreCase))
                {
                    var path = content.Substring(7).Trim();
                    return (Path.GetFullPath(path, dir.FullName), false);
                }
            }

            if (LooksLikeGitDir(iOAbstration, dir.FullName))
            {
                return (dir.FullName, true);
            }

            dir = dir.Parent;
        }

        Console.WriteLine("fatal: not a git repository (or any parent up to mount point /)");
        Environment.Exit(1);

        // Make the compiler happy.
        return (string.Empty, false);
    }

    private static bool LooksLikeGitDir(IIOAbstration iOAbstration, string path)
    {
        return iOAbstration.FileExists(Path.Combine(path, "HEAD"))
            && iOAbstration.DirectoryExists(Path.Combine(path, "objects"))
            && iOAbstration.DirectoryExists(Path.Combine(path, "refs"));
    }
}
