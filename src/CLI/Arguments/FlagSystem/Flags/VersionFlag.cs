namespace Bbranch.CLI.Arguments.FlagSystem.Flags;

public class VersionFlag : IFlag<VersionFlag>
{
    public ArgumentValue Value { get; set; } = ArgumentValue.Emtpy;
}