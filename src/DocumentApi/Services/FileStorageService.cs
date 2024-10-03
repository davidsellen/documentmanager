
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;

namespace DocumentApi.Services;

public class MinioFileStorageSettings
{
    public required string Endpoint { get; set; }
    public required string AccessKey { get; set; }
    public required string SecretKey { get; set; }
    public required string BucketName { get; set; }
}

public class MinioFileStorageService: Interfaces.IFileStorageService
{
    private readonly AmazonS3Client _s3Client;
    private readonly string BucketName;

    public MinioFileStorageService(IOptions<MinioFileStorageSettings> settings)
    {
        BucketName = settings.Value.BucketName;
        var config = new AmazonS3Config
        {
            ServiceURL = settings.Value.Endpoint,  // MinIO endpoint
            ForcePathStyle = true
        };
        _s3Client = new AmazonS3Client(settings.Value.AccessKey, settings.Value.SecretKey, config);
    }

    public async Task<string> UploadFileAsync(string fileName, Stream fileStream)
    {
        var putRequest = new PutObjectRequest
        {
            BucketName = BucketName,
            Key = fileName,
            InputStream = fileStream
        };

        var response = await _s3Client.PutObjectAsync(putRequest);
        return response.VersionId;
    }

    public async Task<Stream> DownloadFileAsync(string fileName)
    {
        var getRequest = new GetObjectRequest
        {
            BucketName = BucketName,
            Key = fileName
        };

        var response = await _s3Client.GetObjectAsync(getRequest);
        return response.ResponseStream;
    }
}
