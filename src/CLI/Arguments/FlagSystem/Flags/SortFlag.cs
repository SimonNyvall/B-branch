namespace Bbranch.CLI.Arguments.FlagSystem.Flags;

public class SortFlag : IFlag<SortFlag>
{
    public ArgumentValue Value { get; set; } = ArgumentValue.Emtpy;
}