namespace DocumentApi.Repositories;

using MongoDB.Driver;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;
using DocumentApi.Models;
using DocumentApi.Services.Interfaces;
using Microsoft.Extensions.Options;

public class DocumentRepository : IDocumentRepository
{
    private readonly IMongoCollection<Document> _documentsCollection;

    public DocumentRepository(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _documentsCollection = database.GetCollection<Document>("Documents");
        
    }

    // Create Document
    public async Task<Document> CreateAsync(Document document)
    {
        await _documentsCollection.InsertOneAsync(document);
        return document;
    }

    // Get Document by Id
    public async Task<Document> GetByIdAsync(string id)
    {
        var filter = Builders<Document>.Filter.Eq(doc => doc.Id, id);
        return await _documentsCollection.Find(filter).FirstOrDefaultAsync();
    }

    // Get all Documents (optional: with filter and pagination)
    public async Task<List<Document>> GetAllAsync(int pageIndex = 0, int pageSize = 10)
    {
        return await _documentsCollection.Find(Builders<Document>.Filter.Empty)
                                          .Skip(pageIndex * pageSize)
                                          .Limit(pageSize)
                                          .ToListAsync();
    }

    // Update Document
    public async Task UpdateAsync(string id, Document updatedDocument)
    {
        var filter = Builders<Document>.Filter.Eq(doc => doc.Id, id);
        await _documentsCollection.ReplaceOneAsync(filter, updatedDocument);
    }

    // Delete Document
    public async Task DeleteAsync(string id)
    {
        var filter = Builders<Document>.Filter.Eq(doc => doc.Id, id);
        await _documentsCollection.DeleteOneAsync(filter);
    }

    // Add Signature to Document
    public async Task AddSignatureAsync(string documentId, Signature signature)
    {
        var filter = Builders<Document>.Filter.Eq(doc => doc.Id, documentId);
        var update = Builders<Document>.Update.Push(doc => doc.Signatures, signature);
        await _documentsCollection.UpdateOneAsync(filter, update);
    }

    // Add AuditLog to Document
    public async Task AddAuditLogAsync(string documentId, AuditLog auditLog)
    {
        var filter = Builders<Document>.Filter.Eq(doc => doc.Id, documentId);
        var update = Builders<Document>.Update.Push(doc => doc.AuditLogs, auditLog);
        await _documentsCollection.UpdateOneAsync(filter, update);
    }
}
