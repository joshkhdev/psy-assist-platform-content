using MongoDB.Bson;
using MongoDB.Bson.IO;
using ContentApi.Core.Domain.Administration;

namespace ContentApi.WebHost.Mapping;

public class ContentMapping : IContentMapping
{
    private readonly JsonWriterSettings _settings;

    public ContentMapping()
    {
        _settings = new JsonWriterSettings() { Indent = true };
    }

    public string CreateMap(IEnumerable<Content> contents)
    {
        return contents.Select(x => x.Info).ToJson(_settings);
    }

    public string CreateMap(Content content)
    {
        return content.Info.ToJson(_settings);
    }
}
