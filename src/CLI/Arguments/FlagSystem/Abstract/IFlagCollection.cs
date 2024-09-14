namespace CLI.Flags;

public interface IFlagCollection
{
    int Count { get; }

    void Add(IFlag flag);

    bool Contains<T>() where T : IFlag;

    bool Contains<T>(out T flag) where T : IFlag;
}