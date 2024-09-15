namespace Bbranch.CLI.Arguments.FlagSystem.Flags;

public class ContainsFlag : IFlag<ContainsFlag>
{
    public ArgumentValue Value { get; set; } = ArgumentValue.Emtpy;
}