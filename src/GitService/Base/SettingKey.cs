namespace Git_Service.Base;

public enum SettingKey
{
    NerdFont,
    MaxAheadBehindDepth,
    Pager,
    DefaultSort
}

public static class SettingKeyExtensions
{
    public static string ToConfigKey(this SettingKey key) => key switch
    {
        SettingKey.NerdFont => "bb.nerdfont",
        SettingKey.MaxAheadBehindDepth => "bb.maxAheadBehindDepth",
        SettingKey.Pager => "bb.pager",
        SettingKey.DefaultSort => "bb.defaultSort",
        _ => throw new ArgumentOutOfRangeException(nameof(key), key, null)
    };
}