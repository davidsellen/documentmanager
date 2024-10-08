using Microsoft.AspNetCore.Mvc;
using DocumentApi.Models;
using DocumentApi.Services.Interfaces;
using DocumentApi.DTOs;
namespace DocumentApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DocumentsController : ControllerBase
{
    private readonly IFileStorageService _fileStorageService;
    private readonly IDocumentService _documentService;

    public DocumentsController(IFileStorageService fileStorageService, IDocumentService documentService)
    {
        _fileStorageService = fileStorageService;
        _documentService = documentService;
    }

    /// <summary>
    /// Uploads a new document and saves its metadata.
    /// </summary>
    /// <param name="file">The file to upload.</param>
    /// <returns>Returns the created document.</returns>
    [HttpPost("upload")]
    public async Task<IActionResult> UploadDocument([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        try
        {
            using (var stream = file.OpenReadStream())
            {
                var documentId = Guid.NewGuid().ToString();
                var uuid = $"{documentId}{Path.GetExtension(file.FileName)}";
                var displayName = Path.GetFileNameWithoutExtension(file.FileName);
                // Upload the file to the file storage service
                var versionId = await _fileStorageService.UploadFileAsync(uuid, stream);

                // Create document metadata
                var document = new Document
                {
                    Id = documentId,
                    DisplayName = displayName,
                    FileName = uuid,
                    VersionId = versionId,
                    ContentType = file.ContentType,
                    CreatedAt = DateTime.UtcNow
                };

                // Save document metadata to the repository
                var createdDocument = await _documentService.CreateDocumentAsync(document);
                return CreatedAtAction(nameof(GetDocumentById), new { id = createdDocument.Id }, createdDocument);
            }
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error uploading file: {ex.Message}");
        }
    }

    /// <summary>
    /// Updates an existing document with a new file.
    /// </summary>
    /// <param name="id">The ID of the document to update.</param>
    /// <param name="file">The new file to upload.</param>
    /// <returns>Returns the updated document.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDocument(string id, [FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        try
        {
            
            var document = await _documentService.GetDocumentByIdAsync(id);
            if (document == null) 
            {
                return BadRequest("File does not exist or it was removed.");
            }

            var displayName = Path.GetFileNameWithoutExtension(file.FileName);

            using var stream = file.OpenReadStream();
            // Upload the new file to the file storage service
            var versionId = await _fileStorageService.UploadFileAsync(document.FileName, stream);
            document.VersionId = versionId;
            document.DisplayName = displayName;

            // Update the document in the repository
            await _documentService.UpdateDocumentAsync(id, document);

            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating file: {ex.Message}");
        }
    }

    /// <summary>
    /// Gets a document by its ID.
    /// </summary>
    /// <param name="id">The ID of the document.</param>
    /// <returns>Returns the document details.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetDocumentById(string id)
    {
        var document = await _documentService.GetDocumentByIdAsync(id);
        if (document == null)
        {
            return NotFound();
        }
        return Ok(document);
    }

    /// <summary>
    /// Gets all documents of current user
    /// </summary>
    /// <returns>Returns a List of DocumentListItem</returns>
    [HttpGet("")]
    public async Task<IActionResult> GetAll()
    {
        var documents = await _documentService.GetAll();
        var listItems = documents
            .Select(document => new DocumentListItem 
            {
                Id = document.Id, 
                DisplayName = document.DisplayName, 
                CreatedAt = document.CreatedAt 
            }).ToList();
        return Ok(listItems);
    }
}