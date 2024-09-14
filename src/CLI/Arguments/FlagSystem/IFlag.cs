namespace Bbranch.CLI.Arguments.FlagSystem;

public interface IFlag
{
    string? Value { get; set; }
}

public interface IFlag<T> : IFlag where T : IFlag<T>, new()
{
    static T Create(string? value)
    {
        T flag = new()
        {
            Value = value
        };

        return flag;
    }
}