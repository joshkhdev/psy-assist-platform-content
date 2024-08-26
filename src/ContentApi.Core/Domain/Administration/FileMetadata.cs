namespace ContentApi.Core.Domain.Administration;

public enum FileType
{
    None = 0,
    Photo,
    Certificate
}

public class FileMetadata
{
    public required string PsyId { get; set; }
    public FileType Type { get; set; }
}
