using System.Text.Json;
using LibGit2Sharp;

namespace Bbranch.GitService.GitRepositories;

public sealed class ExternalGitRepository : IExternalGitRepository
{
    private readonly HttpClient _httpClient;
    private readonly SemaphoreSlim _semaphoreSlim = new(10);
    private readonly RemoteCollection _remote;
    private readonly Dictionary<
        SourceCodeProvider,
        Func<IReadOnlySet<string>, Task<IReadOnlyDictionary<string, string>>>
    > _sourceCodeProviderStrategy;

    public ExternalGitRepository(HttpClient httpClient, string localGitPath) // TODO: make this into a typed httpclient interface for mocking
    {
        _httpClient = httpClient;
        _remote = new Repository(localGitPath).Network.Remotes;
        _sourceCodeProviderStrategy = new Dictionary<
            SourceCodeProvider,
            Func<IReadOnlySet<string>, Task<IReadOnlyDictionary<string, string>>>
        >
        {
            { SourceCodeProvider.Github, GetGithubMergeRequestUrls },
            { SourceCodeProvider.GitLab, GetGitLabMergeRequestUrls },
            { SourceCodeProvider.AzureDevops, GetAzureDevOpsMergeRequestUrls },
        };
    }

    public Task<IReadOnlyDictionary<string, string>> GetMergeRequestUrls(
        IReadOnlySet<string> branchNames
    )
    {
        var provider = GetSourceCodeProvider();

        if (!_sourceCodeProviderStrategy.TryGetValue(provider, out var strategy))
        {
            throw new NotSupportedException($"Source code provider '{provider}' is not supported");
        }

        return strategy(branchNames);
    }

    private SourceCodeProvider GetSourceCodeProvider()
    {
        var remote = _remote["origin"];

        if (remote == null)
            throw new ArgumentException("No 'origin' remote found.");

        var url = remote.Url;

        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("Origin URL is empty.");

        var host = GetRemoteHost(url);

        return host switch
        {
            "github.com" => SourceCodeProvider.Github,
            "gitlab.com" => SourceCodeProvider.GitLab,
            "dev.azure.com" => SourceCodeProvider.AzureDevops,

            _ => throw new NotSupportedException($"Unsupported source code provider: {host}"),
        };
    }

    private static string GetRemoteHost(string remoteUrl)
    {
        // HTTPS:
        // https://github.com/org/repo.git

        if (Uri.TryCreate(remoteUrl, UriKind.Absolute, out var uri))
        {
            return uri.Host.ToLowerInvariant();
        }

        // SSH:
        // git@github.com:org/repo.git

        var atIndex = remoteUrl.IndexOf('@');

        if (atIndex >= 0)
        {
            var colonIndex = remoteUrl.IndexOf(':', atIndex);

            if (colonIndex > atIndex)
            {
                return remoteUrl[(atIndex + 1)..colonIndex].ToLowerInvariant();
            }
        }

        throw new ArgumentException($"Unable to determine provider from remote URL: {remoteUrl}");
    }

    private async Task<IReadOnlyDictionary<string, string>> GetGithubMergeRequestUrls(
        IReadOnlySet<string> branchNames
    )
    {
        var (owner, repo) = GetGithubRepository();

        var url = $"https://api.github.com/repos/{owner}/{repo}/pulls" + "?state=all&per_page=100";

        using var request = new HttpRequestMessage(HttpMethod.Get, url);

        request.Headers.UserAgent.ParseAdd("MyApplication");
        request.Headers.Accept.ParseAdd("application/vnd.github+json");

        var response = await _httpClient.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        using var document = JsonDocument.Parse(json);

        return document
            .RootElement.EnumerateArray()
            .Where(x => x.GetProperty("head").GetProperty("repo").ValueKind != JsonValueKind.Null)
            .GroupBy(x => x.GetProperty("head").GetProperty("ref").GetString()!)
            .ToDictionary(x => x.Key, x => x.First().GetProperty("html_url").GetString()!);
    }

    private (string Owner, string Repostory) GetGithubRepository()
    {
        var remote = _remote["origin"];

        if (remote == null)
            throw new ArgumentException();

        var url = remote.Url;

        if (string.IsNullOrEmpty(url))
            throw new ArgumentNullException("Url is null");

        // HTTPS:
        // https://github.com/my-org/my-repo.git

        // SSH:
        // git@github.com:my-org/my-repo.git

        var uri = url.StartsWith("git@github.com:")
            ? new Uri("https://github.com/" + url["git@github.com".Length])
            : new Uri(url);

        var parts = uri.AbsolutePath.Trim('/').TrimEnd(".git".ToCharArray()).Split('/');

        if (parts.Length > 2)
        {
            throw new ArgumentException("did not work here");
        }

        return (parts[^2], parts[^1]);
    }

    private async Task<IReadOnlyDictionary<string, string>> GetGitLabMergeRequestUrls(
        IReadOnlySet<string> branchNames
    )
    {
        var project = GetGitLabProject();

        var result = new Dictionary<string, string>();

        var httpTasks = new List<(Task<HttpResponseMessage> httpCall, string branchName)>();

        foreach (var branchName in branchNames)
        {
            var url =
                $"https://gitlab.com/api/v4/projects/"
                + $"{Uri.EscapeDataString(project)}/merge_requests"
                + $"?state=merged"
                + $"&source_branch={Uri.EscapeDataString(branchName)}"
                + $"&order_by=merged_at"
                + $"&sort=desc"
                + $"&per_page=1";

            using var request = new HttpRequestMessage(HttpMethod.Get, url);

            var response = _httpClient.SendAsync(request);
            httpTasks.Add((response, branchName));
        }

        await Task.WhenAll(httpTasks.Select(x => x.httpCall));

        foreach (var (response, branchName) in httpTasks)
        {
            var json = await response.Result.Content.ReadAsStringAsync();

            using var document = JsonDocument.Parse(json);

            var mergeRequest = document.RootElement.EnumerateArray().FirstOrDefault();

            if (mergeRequest.ValueKind == JsonValueKind.Undefined)
                continue;

            var mergeRequestUrl = mergeRequest.GetProperty("web_url").GetString();

            if (!string.IsNullOrEmpty(mergeRequestUrl))
            {
                result[branchName] = mergeRequestUrl;
            }
        }

        return result;
    }

    private string GetGitLabProject()
    {
        var remote = _remote["origin"];

        if (remote == null)
            throw new ArgumentException("No 'origin' remote found.");

        var url = remote.Url;

        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("Origin URL is empty.");

        var path = GetRemotePath(url);

        if (path.EndsWith(".git", StringComparison.OrdinalIgnoreCase))
        {
            path = path[..^4];
        }

        return path;
    }

    private async Task<IReadOnlyDictionary<string, string>> GetAzureDevOpsMergeRequestUrls(
        IReadOnlySet<string> branchNames
    )
    {
        var repository = GetAzureDevOpsRepository();

        var result = new Dictionary<string, string>();

        const int pageSize = 100;

        for (var skip = 0; ; skip += pageSize)
        {
            var url =
                $"https://dev.azure.com/{Uri.EscapeDataString(repository.Organization)}"
                + $"/{Uri.EscapeDataString(repository.Project)}"
                + $"/_apis/git/repositories/{Uri.EscapeDataString(repository.Repository)}/pullrequests"
                + $"?searchCriteria.status=completed"
                + $"&$top={pageSize}"
                + $"&$skip={skip}"
                + $"&api-version=7.1";

            using var request = new HttpRequestMessage(HttpMethod.Get, url);

            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            using var document = JsonDocument.Parse(json);

            var pullRequests = document.RootElement.GetProperty("value").EnumerateArray().ToList();

            foreach (var pullRequest in pullRequests)
            {
                var branch = pullRequest.GetProperty("sourceRefName").GetString();

                var pullRequestId = pullRequest.GetProperty("pullRequestId").GetInt32();

                if (string.IsNullOrEmpty(branch))
                    continue;

                branch = branch.Replace("refs/heads/", "");

                var pullRequestUrl =
                    $"https://dev.azure.com/{Uri.EscapeDataString(repository.Organization)}"
                    + $"/{Uri.EscapeDataString(repository.Project)}"
                    + $"/_git/{Uri.EscapeDataString(repository.Repository)}"
                    + $"/pullrequest/{pullRequestId}";

                result.TryAdd(branch, pullRequestUrl);
            }

            if (pullRequests.Count < pageSize)
                break;
        }

        return result;
    }

    private (string Organization, string Project, string Repository) GetAzureDevOpsRepository()
    {
        var remote = _remote["origin"];

        if (remote == null)
            throw new ArgumentException("No 'origin' remote found.");

        var url = remote.Url;

        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("Origin URL is empty.");

        var uri = ParseRemoteUrl(url);

        var parts = uri.AbsolutePath.Trim('/').Split('/');

        // /organization/project/_git/repository
        if (parts.Length != 4 || !parts[2].Equals("_git", StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException($"Invalid Azure DevOps repository URL: {url}");
        }

        return (parts[0], parts[1], parts[3]);
    }

    private static Uri ParseRemoteUrl(string remoteUrl)
    {
        // HTTPS:
        // https://github.com/org/repo.git
        // https://dev.azure.com/org/project/_git/repo

        if (Uri.TryCreate(remoteUrl, UriKind.Absolute, out var uri))
            return uri;

        // SSH:
        // git@github.com:org/repo.git
        // git@gitlab.com:org/repo.git

        var atIndex = remoteUrl.IndexOf('@');
        var colonIndex = remoteUrl.IndexOf(':', atIndex + 1);

        if (atIndex < 0 || colonIndex < 0)
            throw new ArgumentException($"Invalid git remote URL: {remoteUrl}");

        var host = remoteUrl[(atIndex + 1)..colonIndex];
        var path = remoteUrl[(colonIndex + 1)..];

        return new Uri($"https://{host}/{path}");
    }

    private static string GetRemotePath(string remoteUrl)
    {
        return ParseRemoteUrl(remoteUrl).AbsolutePath.Trim('/');
    }

    private enum SourceCodeProvider
    {
        Github,
        GitLab,
        AzureDevops,
    }
}
