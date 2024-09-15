namespace Bbranch.CLI.Arguments.FlagSystem.Flags;

public class QuiteFlag : IFlag<QuiteFlag>
{
    public ArgumentValue Value { get; set; } = ArgumentValue.Emtpy;
}