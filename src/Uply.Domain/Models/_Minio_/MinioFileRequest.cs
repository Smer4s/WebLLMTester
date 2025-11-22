namespace Uply.Domain.Models._Minio_;

public class MinioFileRequest
{
    public string BucketName { get; set; } = null!;

    public string FileName { get; set; } = null!;
}
