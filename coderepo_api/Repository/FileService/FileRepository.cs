namespace coderepo_api.Repository
{
    public class FileRepository : IFileRepository
    {
        private readonly string _basePath;

        public FileRepository(IConfiguration config)
        {
            _basePath = config.GetValue<string>("FileSettings:BasePath") ?? throw new ArgumentNullException(nameof(config), "BasePath configuration is missing.");
        }

        private string GetFullPath(string relativePath)
        {
            return Path.Combine(_basePath, relativePath);
        }

        public async Task CreateDirectoryAsync(string relativePath)
        {
            var fullPath = GetFullPath(relativePath);
            if (!string.IsNullOrEmpty(fullPath) && !Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);
            await Task.CompletedTask;
        }

        public async Task SaveFileAsync(string relativePath, byte[] content)
        {
            var fullPath = GetFullPath(relativePath);
            var dir = Path.GetDirectoryName(fullPath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            await File.WriteAllBytesAsync(fullPath, content);
        }

        public async Task RenameFileAsync(string relativeOldPath, string relativeNewPath)
        {
            var oldFull = GetFullPath(relativeOldPath);
            var newFull = GetFullPath(relativeNewPath);

            var newDir = Path.GetDirectoryName(newFull);
            if (!string.IsNullOrEmpty(newDir) && !Directory.Exists(newDir))
                Directory.CreateDirectory(newDir);

            if (File.Exists(oldFull))
            {
                File.Move(oldFull, newFull);
            }
            await Task.CompletedTask;
        }
    }
}
