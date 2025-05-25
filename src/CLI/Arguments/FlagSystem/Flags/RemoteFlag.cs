namespace Bbranch.CLI.Arguments.FlagSystem.Flags;

public sealed class RemoteFlag : IFlag<RemoteFlag>
{
    public ArgumentValue Value { get; set; } = ArgumentValue.Emtpy;
}