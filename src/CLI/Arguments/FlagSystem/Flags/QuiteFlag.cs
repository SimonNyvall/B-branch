namespace Bbranch.CLI.Arguments.FlagSystem.Flags;

public class quietFlag : IFlag<quietFlag>
{
    public ArgumentValue Value { get; set; } = ArgumentValue.Emtpy;
}