namespace Git_Service.Base.GitSettingsRepository;

public sealed class GitSettingsRepository : IGitSettingsRepository
{
    private readonly Dictionary<SettingKey, ISettingsValue?> _settings = [];
    private static string _globalConfigValue = null!, _repositoryConfigValue = null!;
    private static GitSettingsRepository? _instance = null;

    public static GitSettingsRepository GetInstance()
    {
        if (_instance is not null)
        {
            return _instance;
        }

        lock (typeof(GitSettingsRepository))
        {
            _instance = _instance is null ? new GitSettingsRepository() : _instance;
        }

        return _instance;
    }

    private GitSettingsRepository()
    {
        LoadGlobalConfigValue();
        LoadRepositoryConfigValue();
        SetSettings();
    }

    public T? GetValue<T>(SettingKey key) where T : ISettingsValue
    {
        if (_settings.TryGetValue(key, out var value) && value is T typedValue)
        {
            return typedValue;
        }

        return default;
    }

    private void SetSettings()
    {
        _settings.Add(SettingKey.NerdFont, GetNerdFontSetting());
        _settings.Add(SettingKey.MaxAheadBehindDepth, GetMaxAheadBehindDepthSetting());
        _settings.Add(SettingKey.Pager, GetPagerSetting());
        _settings.Add(SettingKey.DefaultSort, GetDefaultSortSetting());
    }

    private static BoolSettingsValue GetNerdFontSetting()
    {
        var nerdFontValue = FindValueInConfig(_globalConfigValue, SettingKey.NerdFont.ToConfigKey());
        if (string.IsNullOrEmpty(nerdFontValue))
        {
            nerdFontValue = FindValueInConfig(_repositoryConfigValue, SettingKey.NerdFont.ToConfigKey());
        }

        return new BoolSettingsValue(string.Equals(nerdFontValue, "true", StringComparison.OrdinalIgnoreCase));
    }

    private static IntSettingsValue? GetMaxAheadBehindDepthSetting()
    {
        var maxAheadBehindDepthValue = FindValueInConfig(_globalConfigValue, SettingKey.MaxAheadBehindDepth.ToConfigKey());
        if (string.IsNullOrEmpty(maxAheadBehindDepthValue))
        {
            maxAheadBehindDepthValue = FindValueInConfig(_repositoryConfigValue, SettingKey.MaxAheadBehindDepth.ToConfigKey());
        }

        if (int.TryParse(maxAheadBehindDepthValue, out var depth))
        {
            return new IntSettingsValue(depth);
        }

        return null;
    }

    private static StringSettingsValue? GetPagerSetting()
    {
        var pagerValue = FindValueInConfig(_globalConfigValue, SettingKey.Pager.ToConfigKey());
        if (string.IsNullOrEmpty(pagerValue))
        {
            pagerValue = FindValueInConfig(_repositoryConfigValue, SettingKey.Pager.ToConfigKey());
        }

        if (!string.IsNullOrEmpty(pagerValue))
        {
            return new StringSettingsValue(pagerValue);
        }

        return null;
    }

    private static StringSettingsValue? GetDefaultSortSetting()
    {
        var defaultSortValue = FindValueInConfig(_globalConfigValue, SettingKey.DefaultSort.ToConfigKey());
        if (string.IsNullOrEmpty(defaultSortValue))
        {
            defaultSortValue = FindValueInConfig(_repositoryConfigValue, SettingKey.DefaultSort.ToConfigKey());
        }

        if (!string.IsNullOrEmpty(defaultSortValue))
        {
            return new StringSettingsValue(defaultSortValue);
        }

        return null;
    }

    private static void LoadGlobalConfigValue()
    {
        var gitPath = Environment.GetEnvironmentVariable("GIT_CONFIG_GLOBAL") ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".gitconfig");
        _globalConfigValue = gitPath;

        if (!File.Exists(_globalConfigValue))
        {
            Console.WriteLine("fatal: global git config file not found.");
            Environment.Exit(1);
        }

        _globalConfigValue = File.ReadAllText(_globalConfigValue).Trim();
    }

    private static void LoadRepositoryConfigValue()
    {
        var gitPath = GetRepositoryGitPath();
        _repositoryConfigValue = Path.Combine(gitPath, "config");

        if (!File.Exists(_repositoryConfigValue))
        {
            Console.WriteLine("fatal: not a git repository (or any parent up to mount point /)");
            Environment.Exit(1);
        }

        _repositoryConfigValue = File.ReadAllText(_repositoryConfigValue).Trim();
    }

    private static string GetRepositoryGitPath()
    {
        var currentDirectory = Directory.GetCurrentDirectory();

        while (!string.IsNullOrEmpty(currentDirectory))
        {
            var gitPath = Path.Combine(currentDirectory, ".git");

            if (Directory.Exists(gitPath))
            {
                return gitPath;
            }

            if (File.Exists(gitPath))
            {
                var gitFileContent = File.ReadAllText(gitPath).Trim();

                if (gitFileContent.StartsWith("gitdir:"))
                {
                    return Path.GetFullPath(gitFileContent[7..].Trim(), currentDirectory);
                }
            }

            currentDirectory = Directory.GetParent(currentDirectory)?.FullName;
        }

        Console.WriteLine("fatal: not a git repository (or any parent up to mount point /)");
        Environment.Exit(1);
        return string.Empty; // This line will never be reached, but is required to satisfy the compiler.
    }
    
    private static string FindValueInConfig(string configContent, string key)
    {
        var lines = configContent.Split(['\n', '\r'], StringSplitOptions.RemoveEmptyEntries);
        foreach (var line in lines)
        {
            if (line.Trim().StartsWith(key + " = "))
            {
                return line.Substring(key.Length + 3).Trim();
            }
        }
        return string.Empty;
    }
}
