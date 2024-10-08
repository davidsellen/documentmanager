using Microsoft.Identity.Client;

namespace DocumentApi.Models;

public class Document
{
    public required string Id { get; set; }  // Unique document identifier
    public string Name { get; set; }  // Document name (e.g., contract.pdf)
    public string OwnerId { get; set; }  // User ID of the document owner (Uploader)
    public string StoragePath { get; set; }  // Path to the document in cloud storage
    public string ContentType { get; set; }  // MIME type of the document (e.g., application/pdf)
    public long Size { get; set; }  // Size of the document in bytes
    public DateTime CreatedAt { get; set; }  // Upload timestamp
    public DocumentMetadata Metadata { get; set; }  // Metadata associated with the document
    public List<Signature> Signatures { get; set; }  // List of signatures for this document
    public List<AuditLog> AuditLogs { get; set; }  // List of actions taken on the document (audit trail)
    public string DisplayName { get; internal set; }
    public string FileName { get; internal set; }
    public string VersionId { get; internal set; }

    
}

public class DocumentMetadata
{
    public string Description { get; set; }  // Short description of the document
    public List<string> Tags { get; set; }  // Tags for easier categorization and search
    public int Version { get; set; }  // Version number of the document
    public DateTime LastModified { get; set; }  // Timestamp of the last modification
}

public class Signature
{
    public required string Id { get; set; }  // Unique signature identifier
    public required string DocumentId { get; set; }  // Document associated with this signature
    public required string SignerId { get; set; }  // User ID of the signer
    public required string Status { get; set; }  // Status of the signature (Pending, Completed, Rejected)
    public DateTime RequestedAt { get; set; }  // When the signature request was created
    public DateTime? SignedAt { get; set; }  // When the signature was completed
    public required string SignerIp { get; set; }  // IP address of the signer at the time of signing
    public required string SignatureImagePath { get; set; }  // Path to the image of the signature (if applicable)
}

public class AuditLog
{
    public required string Id { get; set; }  // Unique audit log identifier
    public required string DocumentId { get; set; }  // Document associated with the action
    public required string UserId { get; set; }  // User who performed the action
    public required string Action { get; set; }  // Action performed (Uploaded, Signed, Viewed, Shared, Deleted)
    public DateTime ActionTimestamp { get; set; }  // When the action took place
    public required string IpAddress { get; set; }  // IP address of the user who performed the action
    public required string Details { get; set; }  // Additional details about the action (e.g., reason for rejection)
}

public class Share
{
    public required string  Id { get; set; }  // Unique share ID
    public required string DocumentId { get; set; }  // The document being shared
    public required string SharedWithUserId { get; set; }  // The user ID with whom the document is shared
    public required string SharedByUserId { get; set; }  // The user ID of the person sharing the document
    public required string Permission { get; set; }  // Permission type (Read, Write, Sign)
    public DateTime SharedAt { get; set; }  // When the document was shared
    public DateTime? ExpiryDate { get; set; }  // When the share link expires (optional)
}
