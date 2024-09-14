namespace CLI.Flags;

public class ContainsFlag : IFlag<ContainsFlag>
{
    public string? Value { get; set; }
}