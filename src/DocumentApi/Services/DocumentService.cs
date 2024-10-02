using DocumentApi.Models;
using DocumentApi.Repositories;

namespace DocumentApi.Services;

public class DocumentService : Interfaces.IDocumentService
{
    private readonly Interfaces.IDocumentRepository _documentRepository;

    public DocumentService(Interfaces.IDocumentRepository documentRepository)
    {
        _documentRepository = documentRepository;
    }

    public async Task<Document> CreateDocumentAsync(Document document)
    {
        return await _documentRepository.CreateAsync(document);
    }

     public async Task UpdateDocumentAsync(string id, Document document)
    {
        await _documentRepository.UpdateAsync(id, document);
    }

    public async Task<Document> GetDocumentByIdAsync(string documentId)
    {
        return await _documentRepository.GetByIdAsync(documentId);
    }

    public async Task AddSignatureToDocumentAsync(string documentId, Signature signature)
    {
        await _documentRepository.AddSignatureAsync(documentId, signature);
    }

    public async Task LogAuditActionAsync(string documentId, AuditLog auditLog)
    {
        await _documentRepository.AddAuditLogAsync(documentId, auditLog);
    }
}
