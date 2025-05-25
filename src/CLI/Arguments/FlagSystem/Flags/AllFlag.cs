namespace Bbranch.CLI.Arguments.FlagSystem.Flags;

public sealed class AllFlag : IFlag<AllFlag>
{
    public ArgumentValue Value { get; set; } = ArgumentValue.Emtpy;
}