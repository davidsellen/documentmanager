namespace DocumentApi.DTOs;
public record DocumentListItem 
{
    public required string Id { get; set; }
    public required string DisplayName { get; set; }
    public DateTime CreatedAt { get; set; }
}