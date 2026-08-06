namespace Bbranch.GitService;

public interface IIOAbstration
{
    bool FileExists(string path);
    bool DirectoryExists(string path);
    DateTime GetLastWriteTime(string path);
    Task<string[]> ReadAllLines(string path);
    Task<string> ReadAllText(string path);
    Task<byte[]> ReadAllBytes(string path);
    string GetFileName(string path);
    string[] GetDirectories(string path);
    DirectoryInfo? GetCurrentDirectory();
    public string[] GetFiles(string path);
    string? GetRelativePath(string relativeTo, string path);
}

public sealed class IOAbstraction : IIOAbstration
{
    public bool DirectoryExists(string path) => Directory.Exists(path);

    public bool FileExists(string path) => File.Exists(path);

    public string[] GetFiles(string path) =>
        Directory.GetFiles(path, "*", SearchOption.AllDirectories);

    public DirectoryInfo? GetCurrentDirectory() =>
        new DirectoryInfo(Directory.GetCurrentDirectory());

    public string[] GetDirectories(string path) =>
        Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly);

    public string GetFileName(string path) => Path.GetFileName(path);

    public DateTime GetLastWriteTime(string path) => File.GetLastWriteTime(path);

    public Task<byte[]> ReadAllBytes(string path) => File.ReadAllBytesAsync(path);

    public Task<string[]> ReadAllLines(string path) => File.ReadAllLinesAsync(path);

    public Task<string> ReadAllText(string path) => File.ReadAllTextAsync(path);

    public string? GetRelativePath(string relativeTo, string path) =>
        Path.GetRelativePath(relativeTo, path);
}
