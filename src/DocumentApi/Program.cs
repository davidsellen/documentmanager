using DocumentApi.Services;
using DocumentApi.Services.Interfaces;
using DocumentApi.Repositories;


public class Program
{

   
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection(nameof(MongoDbSettings)));
        builder.Services.Configure<MinioSettings>(builder.Configuration.GetSection(nameof(MinioSettings)));
        builder.Services.Configure<OpenAiSettings>(builder.Configuration.GetSection(nameof(OpenAiSettings)));
        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Register services
        //builder.Services.AddSingleton<IAmazonS3, MinioS3Client>(); // Configure MinIO client
        builder.Services.AddScoped<IFileStorageService, MinioFileStorageService>();
        builder.Services.AddSingleton<IDocumentRepository, DocumentRepository>();
        builder.Services.AddScoped<IDocumentService, DocumentService>();
        builder.Services.AddScoped<IChatService, ChatService>();
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