using ContentApi.Core.Domain.Administration;

namespace ContentApi.WebHost.Mapping;

public interface IContentMapping
{
    string CreateMap(IEnumerable<Content> contents);

    string CreateMap(Content content);
}
