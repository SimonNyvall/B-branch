namespace GUI.Storage.Repository;

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public class Repository : IRepository
{
    private readonly string _saveFilePath = "home/sn/.local/share/B-branch/paths.data";

    public bool DoesSaveFileExistAsync()
    {
        return File.Exists(_saveFilePath);
    }

    public void CreateSaveFileAsync()
    {
        File.Create(_saveFilePath);
    }

    public async Task<string[]> GetPathsAsync()
    {
        string[] paths = await File.ReadAllLinesAsync(_saveFilePath);

        return paths;
    }

    public async Task RemovePathFromFileAsync(string path)
    {
        string[] paths = await GetPathsAsync();

        paths = paths.Where(p => p != path).ToArray();

        await File.WriteAllLinesAsync(_saveFilePath, paths);
    }

    public async Task SavePathToFileAsync(string path)
    {
        string[] paths = await GetPathsAsync();

        if (paths.Contains(path))
        {
            throw new InvalidOperationException("Path already exists in file.");
        }

        paths = paths.Append(path).ToArray();

        await File.WriteAllLinesAsync(_saveFilePath, paths);
    }
}