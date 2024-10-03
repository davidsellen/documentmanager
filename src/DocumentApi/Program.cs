using DocumentApi.Services;
using DocumentApi.Models;
using DocumentApi.Services.Interfaces;
using DocumentApi.Repositories;
using Amazon.S3;
using Amazon.S3.Model;
using MongoDB.Driver;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace DocumentApi;

public class Program
{
    public static void Main(string[] args)
    {

       
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection(nameof(MongoDbSettings)));
        builder.Services.Configure<MinioFileStorageSettings>(builder.Configuration.GetSection(nameof(MinioFileStorageSettings)));

        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Register services
        //builder.Services.AddSingleton<IAmazonS3, MinioS3Client>(); // Configure MinIO client
        builder.Services.AddScoped<IFileStorageService, MinioFileStorageService>();
        builder.Services.AddSingleton<IDocumentRepository, DocumentRepository>();
        builder.Services.AddScoped<IDocumentService, DocumentService>();
        builder.Services.AddControllers();
        
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