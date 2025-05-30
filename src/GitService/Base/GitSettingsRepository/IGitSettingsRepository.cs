namespace Git_Service.Base.GitSettingsRepository;

public interface IGitSettingsRepository
{
    T? GetValue<T>(SettingKey key) where T : ISettingsValue;
}
