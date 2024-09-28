using DocumentApi.Models;

namespace DocumentApi.Services.Interfaces;

public interface IDocumentRepository
{

    Task<Document> CreateAsync(Document document);

    // Get Document by Id
    Task<Document> GetByIdAsync(string id);

    // Get all Documents (optional: with filter and pagination)
    Task<List<Document>> GetAllAsync(int pageIndex = 0, int pageSize = 10);

    // Update Document
    Task UpdateAsync(string id, Document updatedDocument);

    // Delete Document
    Task DeleteAsync(string id);

    // Add Signature to Document
    Task AddSignatureAsync(string documentId, Signature signature);

    // Add AuditLog to Document
    Task AddAuditLogAsync(string documentId, AuditLog auditLog);
}