namespace DocumentApi.Services.Interfaces;

public interface IFileStorageService
{
    Task<string> UploadFileAsync(string fileName, Stream fileStream);
    Task<Stream> DownloadFileAsync(string fileName);


}