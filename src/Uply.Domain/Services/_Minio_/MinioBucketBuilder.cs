using Application.Abstractions.Services.Minio;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using Uply.Domain.Services._Minio_;

namespace Infrastructure.Minio;

public class MinioBucketBuilder(IMinioClient minioClient, ILogger<MinioBucketBuilder> logger)
    : IMinioBucketBuilder
{
    public async Task SetupBuckets()
    {
        foreach (var bucketToCreate in MinioService.Buckets)
        {
            var bucketName = bucketToCreate.Name;

            if (await minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucketName)))
            {
                continue;
            }

            logger.LogInformation("Minio bucket {bucketName} does not exist, creation started...", bucketName);

            await minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucketName));

            var publicReadPolicy = $$"""
                                         {
                                             "Version":"2012-10-17",
                                             "Statement":[
                                                 {
                                                     "Effect":"Allow",
                                                     "Principal":{"AWS":["*"]},
                                                     "Action":["s3:GetObject"],
                                                     "Resource":["arn:aws:s3:::{{bucketName}}/*"]
                                                 }
                                             ]
                                         }
                                         """;

            await minioClient.SetPolicyAsync(
                new SetPolicyArgs()
                    .WithBucket(bucketName)
                    .WithPolicy(publicReadPolicy));

            logger.LogInformation("Minio bucket {bucketName} created successfully...", bucketName);
        }
    }
}