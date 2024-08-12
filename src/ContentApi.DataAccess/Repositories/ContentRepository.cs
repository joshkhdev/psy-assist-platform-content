using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using ContentApi.Core.Abstractions.Repositories;
using ContentApi.Core.Domain.Administration;

namespace ContentApi.DataAccess.Repositories
{
    public class ContentRepository<T> : IRepository<T> where T : Content
    {
        private readonly DataContext _dataContext;

        public ContentRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IEnumerable<T>> GetAllFilesInfoAsync(CancellationToken cancellationToken)
        {
            var infos = await _dataContext.InfoCollection.FindAsync(_ => true);
            var list = infos.ToList(cancellationToken: cancellationToken).ConvertAll(x => new Content { Info = x });
            return (IEnumerable<T>)list;
        }

        public async Task<T> GetFileInfoByIdAsync(string id, CancellationToken cancellationToken)
        {
            var filter = InitFilter(id);
            return await GetFileInfo(filter, cancellationToken);
        }

        public async Task<IEnumerable<T>> GetFilesInfoByFilterAsync(FileMetadata metadata, CancellationToken cancellationToken)
        {
            var filter = InitFilter(metadata);
            return await GetFilesInfo(filter, cancellationToken);
        }

        public async Task<T> DownLoadFileByIdAsync(string id, CancellationToken cancellationToken)
        {
            var filter = InitFilter(id);
            return await DownLoadFile(filter, cancellationToken);
        }

        public async Task<IEnumerable<T>> DownLoadFilesByFilterAsync(FileMetadata metadata, CancellationToken cancellationToken)
        {
            var filter = InitFilter(metadata);
            return await DownLoadFiles(filter, cancellationToken);
        }

        public async Task DeleteFileByIdAsync(string id, CancellationToken cancellationToken)
        {
            await _dataContext.Bucket.DeleteAsync(new ObjectId(id), cancellationToken);
        }

        public async Task DeleteFilesByFilterAsync(FileMetadata metadata, CancellationToken cancellationToken)
        {
            var filter = InitFilter(metadata);
            var files = await _dataContext.InfoCollection.FindAsync(filter, cancellationToken: cancellationToken);
            foreach (var file in files.ToList())            
                await _dataContext.Bucket.DeleteAsync(file.Id, cancellationToken);           
        }

        public async Task<string> UploadFileAsync(string filename, FileMetadata metadata, byte[] bytes, CancellationToken cancellationToken)
        {
            var options = new GridFSUploadOptions
            {
                Metadata = metadata.ToBsonDocument()
            };

            var id = await _dataContext.Bucket.UploadFromBytesAsync(filename, bytes, options, cancellationToken);
            return id.ToString();
        }

        private async Task<T> DownLoadFile(FilterDefinition<GridFSFileInfo> filter, CancellationToken cancellationToken)
        {
            var infos = await _dataContext.InfoCollection.FindAsync(filter, cancellationToken: cancellationToken);
            var fileInfo = infos.FirstOrDefault(cancellationToken: cancellationToken);
            if (fileInfo == null) return (T)new Content();
            var bytes = await _dataContext.Bucket.DownloadAsBytesAsync(fileInfo.Id, cancellationToken: cancellationToken);
            return (T)new Content() { Bytes = bytes, Info = fileInfo };
        }

        private async Task<IEnumerable<T>> DownLoadFiles(FilterDefinition<GridFSFileInfo> filter, CancellationToken cancellationToken)
        {
            var infos = await _dataContext.InfoCollection.FindAsync(filter, cancellationToken: cancellationToken);
            var list = new List<Content>();
            foreach (var fileInfo in infos.ToList(cancellationToken: cancellationToken))
            {
                var content = new Content()
                {
                    Info = fileInfo,
                    Bytes = await _dataContext.Bucket.DownloadAsBytesAsync(fileInfo.Id, cancellationToken: cancellationToken)
                };
                list.Add(content);
            }
            return (IEnumerable<T>)list;
        }

        private async Task<T> GetFileInfo(FilterDefinition<GridFSFileInfo> filter, CancellationToken cancellationToken)
        {
            var infos = await _dataContext.InfoCollection.FindAsync(filter, cancellationToken: cancellationToken);
            return (T)new Content() { Info = infos.FirstOrDefault(cancellationToken: cancellationToken) };
        }

        private async Task<IEnumerable<T>> GetFilesInfo(FilterDefinition<GridFSFileInfo> filter, CancellationToken cancellationToken)
        {
            var infos = await _dataContext.InfoCollection.FindAsync(filter, cancellationToken: cancellationToken);
            var list = infos.ToList(cancellationToken: cancellationToken).ConvertAll(x => new Content { Info = x });
            return (IEnumerable<T>)list;
        }

        private FilterDefinition<GridFSFileInfo> InitFilter(string id)
        {
            return Builders<GridFSFileInfo>.Filter.Eq("_id", new ObjectId(id));
        }

        private FilterDefinition<GridFSFileInfo> InitFilter(FileMetadata metadata)
        {
            return Builders<GridFSFileInfo>.Filter.And(
                Builders<GridFSFileInfo>.Filter.Eq("metadata.PsyId", metadata.PsyId),
                Builders<GridFSFileInfo>.Filter.Eq("metadata.Type", (int)metadata.Type));
        }
    }
}