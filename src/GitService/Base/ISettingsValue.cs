namespace Git_Service.Base;

public interface ISettingsValue;

public sealed record IntSettingsValue(int Value) : ISettingsValue;

public sealed record StringSettingsValue(string Value) : ISettingsValue;

public sealed record BoolSettingsValue(bool Value) : ISettingsValue;