namespace ContentApi.DataAccess.Data;

public class ContentInitializer : IDbInitializer
{
    private readonly DataContext _dataContext;

    public ContentInitializer(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public void InitializeDb()
    {
        _dataContext.Client.DropDatabase(_dataContext.Db.DatabaseNamespace.DatabaseName);
        _dataContext.Bucket.UploadFromBytes(FakeDataFactory.FileName, FakeDataFactory.ContentSample.Bytes, FakeDataFactory.UploadOptions);
    }
}