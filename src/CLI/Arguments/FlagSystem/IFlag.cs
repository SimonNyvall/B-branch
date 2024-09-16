namespace Bbranch.CLI.Arguments.FlagSystem;

public interface IFlag
{
    ArgumentValue Value { get; set; }
}

public class ArgumentValue
{
    private string Value { get; }

    private ArgumentValue(string value) => Value = value;

    public static implicit operator ArgumentValue(string value) => new(value);

    public override string ToString() => Value;

    public static ArgumentValue Emtpy => new(string.Empty);
}

public interface IFlag<T> : IFlag where T : IFlag<T>, new()
{
    static T Create(string? value)
    {
        if (value is null)
        {
            return new T();
        }

        T flag = new()
        {
            Value = value
        };

        return flag;
    }
}