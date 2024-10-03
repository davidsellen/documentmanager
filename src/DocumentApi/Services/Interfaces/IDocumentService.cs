using DocumentApi.Models;
namespace DocumentApi.Services.Interfaces;

public interface IDocumentService 
{
    Task<Document> CreateDocumentAsync(Document document);
    Task UpdateDocumentAsync(string id, Document document);
    Task<Document> GetDocumentByIdAsync(string documentId);
    Task AddSignatureToDocumentAsync(string documentId, Signature signature);
    Task LogAuditActionAsync(string documentId, AuditLog auditLog);
    Task<IEnumerable<Document>> GetAll();
}