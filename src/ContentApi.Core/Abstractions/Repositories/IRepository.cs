using ContentApi.Core.Domain.Administration;

namespace ContentApi.Core.Abstractions.Repositories
{
    public interface IRepository<T> where T : Content
    {
        Task<IEnumerable<T>> GetAllFilesInfoAsync(CancellationToken cancellationToken);

        Task<T> GetFileInfoByIdAsync(string id, CancellationToken cancellationToken);

        Task<IEnumerable<T>> GetFilesInfoByFilterAsync(FileMetadata metadata, CancellationToken cancellationToken);

        Task<T> DownLoadFileByIdAsync(string id, CancellationToken cancellationToken);

        Task<IEnumerable<T>> DownLoadFilesByFilterAsync(FileMetadata metadata, CancellationToken cancellationToken);

        Task DeleteFileByIdAsync(string id, CancellationToken cancellationToken);

        Task DeleteFilesByFilterAsync(FileMetadata metadata, CancellationToken cancellationToken);

        Task<string> UploadFileAsync(string filename, FileMetadata metadata, byte[] bytes, CancellationToken cancellationToken);
    }
}
