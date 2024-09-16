namespace Bbranch.CLI.Arguments.FlagSystem.Flags;

public class TrackFlag : IFlag<TrackFlag>
{
    public ArgumentValue Value { get; set; } = ArgumentValue.Emtpy;
}