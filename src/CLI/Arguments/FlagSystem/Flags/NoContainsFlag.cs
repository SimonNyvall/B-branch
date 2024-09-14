namespace CLI.Flags;

public class NoContainsFlag : IFlag<NoContainsFlag>
{
    public string? Value { get; set; }
}