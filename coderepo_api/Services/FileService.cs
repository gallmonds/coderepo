using coderepo_api.Interfaces;
using coderepo_api.Repository;

namespace coderepo_api.Services
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _fileRepository;

        public FileService(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }

        public async Task CreateDirectoryAsync(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                throw new ArgumentException("La ruta no puede estar vacía");


            await _fileRepository.CreateDirectoryAsync(relativePath);
        }

        public async Task SaveFileAsync(string relativePath, byte[] content)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                throw new ArgumentException("La ruta no puede estar vacía");

            if (content == null || content.Length == 0)
                throw new ArgumentException("El archivo está vacío");

            await _fileRepository.SaveFileAsync(relativePath, content);
        }

        public async Task RenameFileAsync(string relativeOldPath, string relativeNewPath)
        {
            if (string.IsNullOrWhiteSpace(relativeOldPath) || string.IsNullOrWhiteSpace(relativeNewPath))
                throw new ArgumentException("Las rutas no pueden estar vacías");

            await _fileRepository.RenameFileAsync(relativeOldPath, relativeNewPath);
        }
    }
}
