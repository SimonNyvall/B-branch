namespace Bbranch.CLI.Arguments.FlagSystem.Flags;

public sealed class VersionFlag : IFlag<VersionFlag>
{
    public ArgumentValue Value { get; set; } = ArgumentValue.Emtpy;
}