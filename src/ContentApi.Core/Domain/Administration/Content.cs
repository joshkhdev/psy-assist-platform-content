using MongoDB.Driver.GridFS;

namespace ContentApi.Core.Domain.Administration
{
    public enum FileType
    {
        None = 0,
        Photo,
        Certificate
    }

    public class FileMetadata
    {
        public string PsyId { get; set; }
        public FileType Type { get; set; }
    }

    public class Content : BaseEntity
    {
        public GridFSFileInfo Info { get; set; }
        public byte[] Bytes { get; set; }
    }
}
