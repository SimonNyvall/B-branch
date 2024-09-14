namespace CLI.Flags;

public class VersionFlag : IFlag<VersionFlag>
{
    public string? Value { get; set; }
}