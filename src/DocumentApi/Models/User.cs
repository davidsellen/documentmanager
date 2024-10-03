namespace DocumentApi.Models;

public class User
{
    public required string Id { get; set; }  // Unique user identifier
    public string? Email { get; set; }  // Email of the user
    public required string Name { get; set; }  // Full name of the user
    //public required string Role { get; set; }  // Role of the user (Admin, Signer, Viewer)
}