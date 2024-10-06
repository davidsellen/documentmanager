
using System.Collections.Immutable;
using DocumentApi.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.KernelMemory;

namespace DocumentApi.Services;

public class OpenAiSettings
{
    public required string SecretKey { get; set; }
}

public class ChatService : IChatService
{
    private readonly IFileStorageService fileStorageService;
    private readonly IDocumentService documentService;
    private OpenAiSettings openAiSettings;
    private IKernelMemory? kernelMemory;
    public ChatService(
        IOptions<OpenAiSettings> options
    , IFileStorageService fileStorageService
    , IDocumentService documentService)
    {
        openAiSettings =  new OpenAiSettings 
        {
            SecretKey = options.Value.SecretKey
        };
        this.fileStorageService = fileStorageService;
        this.documentService = documentService;
    }

    public async Task<string> AskAsync(string question) 
    {
        await InitAsync();

        if (kernelMemory == null) 
        {
            throw new ArgumentException("Could not initialize the chat");
        }

        var responses = await kernelMemory.AskAsync(question);

        return responses?.ToString() ?? string.Empty;
    } 

    public async Task InitAsync() 
    {

        if (kernelMemory != null) 
        {
            return;
        }

        var memory = new KernelMemoryBuilder()
            .WithOpenAIDefaults(openAiSettings.SecretKey)
            .Build<MemoryServerless>();

        var allDocuments = await documentService.GetAll();

        foreach(var document in allDocuments) 
        {
            var content = await fileStorageService.DownloadFileAsync(document.FileName);
            
            await memory.ImportDocumentAsync(
                content, 
                tags: CreateTagCollection(document), 
                fileName: document.FileName, 
                documentId: document.Id
            );

        }

        kernelMemory = memory;
    }

    private TagCollection? CreateTagCollection(Models.Document document)
    {
        if (document.Metadata  == null|| document.Metadata.Tags == null) 
        {
            return null;
        }

        var tagDict = document.Metadata.Tags.Distinct().ToImmutableArray().ToDictionary<string, string>(k=>k);

        var tagCollection = new  TagCollection();
        foreach( var tag in document.Metadata.Tags.Distinct())
        {
            tagCollection.Add(tag);
        }
        return tagCollection;
    }
}