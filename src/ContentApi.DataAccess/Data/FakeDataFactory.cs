using MongoDB.Bson;
using MongoDB.Driver.GridFS;
using ContentApi.Core.Domain.Administration;

namespace ContentApi.DataAccess.Data;

public static class FakeDataFactory
{
    private static readonly string image = "iVBORw0KGgoAAAANSUhEUgAAAZAAAAEsAQMAAADXeXeBAAAABGdBTUEAALGPC/xhBQAAACBjSFJNAAB6JgAAgIQAAPoAAACA6AAAdTAAAOpgAAA6mAAAF3CculE8AAAABlBMVEUAAP7////DYP5JAAAAAWJLR0QB/wIt3gAAAAlwSFlzAAALEgAACxIB0t1+/AAAAAd0SU1FB+QIGBcKN7/nP/UAAAAmSURBVGje7cExAQAAAMKg9U9tCU+gAAAAAAAAAAAAAAAAAACApwE7xAABJ3+eCQAAABl0RVh0Y29tbWVudABDcmVhdGVkIHdpdGggR0lNUOevQMsAAAAldEVYdGRhdGU6Y3JlYXRlADIwMjAtMDgtMjRUMjM6MTA6NTUrMDM6MDCQd165AAAAJXRFWHRkYXRlOm1vZGlmeQAyMDIwLTA4LTI0VDIzOjEwOjU1KzAzOjAw4SrmBQAAAABJRU5ErkJggg==";

    private static readonly Dictionary<string, BsonValue> values = new()
    {
        { "PsyId", "user1234"},
        { "Type",  1}
    };

    public static string FileName => "sample.png";

    public static GridFSUploadOptions UploadOptions => new()
    {            
        Metadata = new BsonDocument(values)
    };

    public static Content ContentSample => new()
    {
        Bytes = Convert.FromBase64String(image)
    };
}
