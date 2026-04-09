namespace Bbranch.CLI.Arguments.FlagSystem.Flags;

public sealed class ContainsFlag : IFlag<ContainsFlag>
{
    public ArgumentValue Value { get; set; } = ArgumentValue.Emtpy;
}