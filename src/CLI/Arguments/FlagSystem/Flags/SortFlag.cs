namespace Bbranch.CLI.Arguments.FlagSystem.Flags;

public sealed class SortFlag : IFlag<SortFlag>
{
    public ArgumentValue Value { get; set; } = ArgumentValue.Emtpy;
}