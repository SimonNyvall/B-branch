namespace GUI.Storage.Repository;

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public class Repository : IRepository
{
    private string SaveFilePath
    {
        get
        {
            string userName = Environment.UserName;

            return Environment.OSVersion.Platform switch
            {
                PlatformID.Unix => $"/home/{userName}/.local/share/B-branch/paths",
                PlatformID.Win32NT => $"C:\\Users\\{userName}\\AppData\\Local\\B-branch\\paths",
                PlatformID.MacOSX => $"/Users/{userName}/Library/Application Support/B-branch/paths",
                _ => throw new InvalidOperationException("Unsupported OS.")
            };
        }
    }

    public bool DoesSaveFileExist()
    {
        return File.Exists(SaveFilePath);
    }

    public void CreateSaveFile()
    {
        string directoryPath = SaveFilePath.Substring(0, SaveFilePath.LastIndexOf('/'));

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        File.Create(SaveFilePath);
    }

    public async Task<string[]> GetPathsAsync()
    {
        string[] paths = await File.ReadAllLinesAsync(SaveFilePath);

        return paths;
    }

    public async Task RemovePathFromFileAsync(string path)
    {
        string[] paths = await GetPathsAsync();

        paths = paths.Where(p => p != path).ToArray();

        await File.WriteAllLinesAsync(SaveFilePath, paths);
    }

    public async Task SavePathToFileAsync(string path)
    {
        string[] paths = await GetPathsAsync();

        if (paths.Contains(path))
        {
            throw new InvalidOperationException("Path already exists in file.");
        }

        paths = paths.Append(path).ToArray();

        File.WriteAllLines(SaveFilePath, paths);
    }
}