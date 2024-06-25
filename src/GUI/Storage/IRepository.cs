namespace GUI.Storage.Repository;

using System.Threading.Tasks;

public interface IRepository
{
    bool DoesSaveFileExistAsync();

    void CreateSaveFileAsync();

    Task<string[]> GetPathsAsync();

    Task SavePathToFileAsync(string path);

    Task RemovePathFromFileAsync(string path);
}