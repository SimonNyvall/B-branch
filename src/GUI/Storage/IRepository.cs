using System.Threading.Tasks;

namespace GUI.Storage.Repository;

public interface IRepository
{
    bool DoesSaveFileExist();

    void CreateSaveFile();

    Task<string[]> GetPathsAsync();

    Task SavePathToFileAsync(string path);

    Task RemovePathFromFileAsync(string path);
}