public interface IPager
{
    Task StartLess(IAsyncEnumerable<string> batches, string lessCommandPath);
}
