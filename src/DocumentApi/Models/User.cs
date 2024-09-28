namespace DocumentApi.Models;

public class User
{
    public string Id { get; set; }  // Unique user identifier
    public string Email { get; set; }  // Email of the user
    public string Name { get; set; }  // Full name of the user
    public string Role { get; set; }  // Role of the user (Admin, Signer, Viewer)
}