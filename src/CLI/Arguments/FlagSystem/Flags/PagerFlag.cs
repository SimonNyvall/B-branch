namespace Bbranch.CLI.Arguments.FlagSystem.Flags;

public class PagerFlag : IFlag<PagerFlag>
{
    public ArgumentValue Value { get; set; } = ArgumentValue.Emtpy;
}