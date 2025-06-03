namespace coderepo_api.Interfaces
{
    public interface IFileService
    {
        Task CreateDirectoryAsync(string relativePath);
        Task SaveFileAsync(string relativePath, byte[] content);
        Task RenameFileAsync(string relativeOldPath, string relativeNewPath);
    }
}
