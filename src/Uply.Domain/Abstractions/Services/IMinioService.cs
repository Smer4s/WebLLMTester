using Uply.Domain.Models._Minio_;

namespace Uply.Domain.Abstractions.Services;

public interface IMinioService
{
    Task<string> GetSignedUrlForUploadingFile(string mimeType, bool isPublic, string? fileName = null);

    Task<string> GetSignedUrlForFetchingFile(MinioFileRequest request);

    Task<string?> GetSignedUrlForFetchingFile(string? uploadFileUrl);

    Task<byte[]> DownloadFile(MinioFileRequest request);

    Task DeleteFile(MinioFileRequest request);

    Task DeleteFile(string fileUrl);

    Task<bool> IsFileExist(string fileUrl);

    Task<bool> IsFileExist(MinioFileRequest fileUrl);

    MinioFileRequest ParseMinioUrl(string minioUrl);

    Task UploadFile(Stream stream, string bucketName, string fileName, CancellationToken cancellationToken = default);
}
