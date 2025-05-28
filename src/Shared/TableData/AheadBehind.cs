namespace Bbranch.Shared.TableData;

public readonly struct AheadBehind
{
    public int Ahead { get; }
    public int Behind { get; }

    public AheadBehind(int ahead, int behind)
    {
        Ahead = ahead;
        Behind = behind;
    }
}