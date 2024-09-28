using DocumentApi.Services;
using DocumentApi.Models;
using DocumentApi.Services.Interfaces;
using DocumentApi.Repositories;
using Amazon.S3;
using Amazon.S3.Model;
using MongoDB.Driver;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Register services
        //builder.Services.AddSingleton<IAmazonS3, MinioS3Client>(); // Configure MinIO client
        builder.Services.AddScoped<IFileStorageService, MinioFileStorageService>();
        builder.Services.AddScoped<IDocumentService, DocumentService>();
        builder.Services.AddScoped<IDocumentRepository, DocumentRepository>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.MapControllers();

        app.Run();
    }
}

public record LoginRequest(string email, string password);
public record SignatureRequest(string email, string message);