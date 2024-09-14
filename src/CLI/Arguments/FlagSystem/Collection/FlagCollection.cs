namespace CLI.Flags;

public class FlagCollection : IFlagCollection
{
    private readonly List<IFlag> _flags = [];

    public int Count => _flags.Count;

    public void Add(IFlag flag) => _flags.Add(flag);

    public bool Contains<T>() where T : IFlag => _flags.Any(f => f is T);

    public bool Contains<T>(out T flag) where T : IFlag
    {
        if (_flags.Any(f => f is T))
        {
            flag = (T)_flags.First(f => f is T);
            return true;
        }

        flag = default!;
        return false;
    }
}