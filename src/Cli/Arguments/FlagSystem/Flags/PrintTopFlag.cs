namespace Bbranch.CLI.Arguments.FlagSystem.Flags;

public sealed class PrintTopFlag : IFlag<PrintTopFlag>
{
    public ArgumentValue Value { get; set; } = ArgumentValue.Emtpy;
}