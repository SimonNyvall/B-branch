namespace Bbranch.CLI.Arguments.FlagSystem.Flags;

public sealed class NoPagerFlag : IFlag<NoPagerFlag>
{
    public ArgumentValue Value { get; set; } = ArgumentValue.Emtpy;
}