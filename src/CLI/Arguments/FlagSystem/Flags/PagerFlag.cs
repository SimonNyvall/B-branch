namespace Bbranch.CLI.Arguments.FlagSystem.Flags;

public sealed class PagerFlag : IFlag<PagerFlag>
{
    public ArgumentValue Value { get; set; } = ArgumentValue.Emtpy;
}