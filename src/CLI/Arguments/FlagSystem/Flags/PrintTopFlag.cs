namespace Bbranch.CLI.Arguments.FlagSystem.Flags;

public class PrintTopFlag : IFlag<PrintTopFlag>
{
    public ArgumentValue Value { get; set; } = ArgumentValue.Emtpy;
}