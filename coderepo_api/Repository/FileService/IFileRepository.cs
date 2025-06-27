namespace coderepo_api.Repository
{
    public interface IFileRepository
    {
        Task CreateDirectoryAsync(string relativePath);
        Task SaveFileAsync(string relativePath, byte[] content);
        Task RenameFileAsync(string relativeOldPath, string relativeNewPath);
        string GetBasePath();

    }
}
