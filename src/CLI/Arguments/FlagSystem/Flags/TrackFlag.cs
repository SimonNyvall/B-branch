namespace Bbranch.CLI.Arguments.FlagSystem.Flags;

public sealed class TrackFlag : IFlag<TrackFlag>
{
    public ArgumentValue Value { get; set; } = ArgumentValue.Emtpy;
}