
using Amazon.S3;
using Amazon.S3.Model;
using System.IO;
using System.Threading.Tasks;

namespace DocumentApi.Services;


public class MinioFileStorageService: Interfaces.IFileStorageService
{
    private readonly AmazonS3Client _s3Client;
    private const string BucketName = "documents";

    public MinioFileStorageService()
    {
        var config = new AmazonS3Config
        {
            ServiceURL = "http://localhost:9000",  // MinIO endpoint
            ForcePathStyle = true
        };
        _s3Client = new AmazonS3Client("minioadmin", "minioadmin", config);
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
