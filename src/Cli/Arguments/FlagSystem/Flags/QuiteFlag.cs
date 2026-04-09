namespace Bbranch.CLI.Arguments.FlagSystem.Flags;

public sealed class quietFlag : IFlag<quietFlag>
{
    public ArgumentValue Value { get; set; } = ArgumentValue.Emtpy;
}