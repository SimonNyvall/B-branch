namespace Bbranch.Flags;

public enum FlagType
{
    All,
    Help,
    Quiet,
    Sort,
    Contains,
    NoContains,
    Remote,
    Track,
    Version,
    PrintTop
}

/// <summary>
/// Should not be used directly, use <see cref="FlagType"/> instead.
/// </summary>
public enum ShortFlagType
{
    a,
    h,
    q,
    s,
    c,
    nc,
    r,
    t,
    v,
    p
}