namespace Bbranch.CLI.Arguments.FlagSystem.Flags;

public class NoPagerFlag : IFlag<NoPagerFlag>
{
    public ArgumentValue Value { get; set; } = ArgumentValue.Emtpy;
}