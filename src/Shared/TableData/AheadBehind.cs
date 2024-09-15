namespace Bbranch.Shared.TableData;

public struct AheadBehind
{
    public int Ahead { get; set; }
    public int Behind { get; set; }

    public AheadBehind(int ahead, int behind)
    {
        Ahead = ahead;
        Behind = behind;
    }
}