namespace Infrastructure.Minio;

public record MinioBucket(string Name, bool IsPublic, HashSet<string> MimeTypes);