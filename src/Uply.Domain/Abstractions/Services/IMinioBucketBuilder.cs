namespace Application.Abstractions.Services.Minio
{
    public interface IMinioBucketBuilder
    {
        Task SetupBuckets();
    }
}