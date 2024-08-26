using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace ContentApi.DataAccess;

public class DataContext : DbContext
{
    private readonly IMongoDatabase _database;

    private readonly MongoClient _client;

    private readonly GridFSBucket _bucket;

    private readonly IMongoCollection<GridFSFileInfo> _collection;

    public DataContext(IOptions<DatabaseSettings> databaseSettings)
    {
        _client = new MongoClient(databaseSettings.Value.ConnectionString);
        _database = _client.GetDatabase(databaseSettings.Value.DatabaseName);
        var gridFSBucketOptions = new GridFSBucketOptions()
        {
            BucketName = "Content"
        };
        _bucket = new GridFSBucket(_database, gridFSBucketOptions);
        _collection = _database.GetCollection<GridFSFileInfo>("Content.files");
    }

    public IMongoDatabase Db => _database;

    public MongoClient Client => _client;

    public GridFSBucket Bucket => _bucket;

    public IMongoCollection<GridFSFileInfo> InfoCollection => _collection;
}
