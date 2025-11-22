using Infrastructure.Minio;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;
using Uply.Domain.Abstractions.Services;
using Uply.Domain.Models._Minio_;

namespace Uply.Domain.Services._Minio_;

public class MinioService(IMinioClient minioClient) : IMinioService
{
    public static readonly MinioBucket[] Buckets =
        [
            new(BucketNames.PublicImageBucket, true, [.. MimeTypes.ImageTypes])
        ];

    private const int ExpiryTimeSec = 60 * 60 * 24;

    public Task UploadFile(Stream stream, string bucketName, string fileName, CancellationToken cancellationToken = default)
    {
        var putObjectArgs = new PutObjectArgs()
            .WithBucket(bucketName)
            .WithObject(fileName)
            .WithStreamData(stream)
            .WithObjectSize(stream.Length);

        return minioClient.PutObjectAsync(putObjectArgs, cancellationToken);
    }

    public async Task<string?> GetSignedUrlForFetchingFile(string? uploadFileUrl)
    {
        if (string.IsNullOrEmpty(uploadFileUrl))
        {
            return null;
        }

        var request = ParseMinioUrl(uploadFileUrl);

        return await GetSignedUrlForFetchingFile(request);
    }

    public Task<string> GetSignedUrlForFetchingFile(MinioFileRequest request)
    {
        var args = new PresignedGetObjectArgs()
            .WithBucket(request.BucketName)
            .WithObject(request.FileName)
            .WithExpiry(ExpiryTimeSec);

        return minioClient.PresignedGetObjectAsync(args);
    }

    public async Task<string> GetSignedUrlForUploadingFile(string mimeType, bool isPublic, string? fileName = null)
    {
        var bucketName = ToBucketName(mimeType, isPublic);

        var args = new PresignedPutObjectArgs()
            .WithBucket(bucketName)
            .WithObject(fileName is null ? Guid.NewGuid().ToString() : fileName)
            .WithExpiry(ExpiryTimeSec);

        var url = await minioClient.PresignedPutObjectAsync(args);

        return url;
    }

    public async Task<byte[]> DownloadFile(MinioFileRequest request)
    {
        var buffer = new MemoryStream();
        var args = new GetObjectArgs()
            .WithBucket(request.BucketName)
            .WithObject(request.FileName)
            .WithCallbackStream(stream =>
            {
                stream.CopyTo(buffer);
            });

        await minioClient.GetObjectAsync(args);

        var bytes = buffer.ToArray();
        await buffer.DisposeAsync();

        return bytes;
    }

    public Task DeleteFile(MinioFileRequest request)
    {
        var args = new RemoveObjectArgs()
            .WithBucket(request.BucketName)
            .WithObject(request.FileName);

        return minioClient.RemoveObjectAsync(args);
    }

    public async Task DeleteFile(string fileUrl)
    {
        var fileRequest = ParseMinioUrl(fileUrl);

        await DeleteFile(fileRequest);
    }

    public Task<bool> IsFileExist(string fileUrl)
    {
        var fileRequest = ParseMinioUrl(fileUrl);

        return IsFileExist(fileRequest);
    }

    public async Task<bool> IsFileExist(MinioFileRequest fileRequest)
    {
        var args = new StatObjectArgs()
            .WithBucket(fileRequest.BucketName)
            .WithObject(fileRequest.FileName);
        try
        {
            var obj = await minioClient.StatObjectAsync(args);

            return obj.ETag != null;

        }
        catch (ObjectNotFoundException)
        {
            return false;
        }

    }

    public MinioFileRequest ParseMinioUrl(string minioUrl)
    {
        var segments = new Uri(minioUrl).LocalPath
            .Split("/")
            .Where(x => !string.IsNullOrEmpty(x))
            .ToArray();

        return new MinioFileRequest
        {
            BucketName = segments[0],
            FileName = segments[1],
        };
    }

    private static string ToBucketName(string mimeType, bool isPublic)
    {
        var bucket = Buckets.FirstOrDefault(b => b.MimeTypes.Contains(mimeType) && b.IsPublic == isPublic) ?? throw new Exception($"Mime type {mimeType} is not supported");

        return bucket.Name;
    }
}
