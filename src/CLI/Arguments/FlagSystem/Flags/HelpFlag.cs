namespace Bbranch.CLI.Arguments.FlagSystem.Flags;

public sealed class HelpFlag : IFlag<HelpFlag>
{
    public ArgumentValue Value { get; set; } = ArgumentValue.Emtpy;
}