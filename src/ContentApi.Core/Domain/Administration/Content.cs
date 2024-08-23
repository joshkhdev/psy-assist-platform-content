using MongoDB.Driver.GridFS;

namespace ContentApi.Core.Domain.Administration;

public class Content : BaseEntity
{
    public GridFSFileInfo Info { get; set; }
    public byte[] Bytes { get; set; }
}
