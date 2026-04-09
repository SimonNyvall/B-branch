namespace Bbranch.CLI.Arguments.FlagSystem.Flags;

public sealed class NoContainsFlag : IFlag<NoContainsFlag>
{
    public ArgumentValue Value { get; set; } = ArgumentValue.Emtpy;
}