namespace DocumentApi.Services.Interfaces;

public interface IChatService
{
    Task<string> AskAsync(string question) ;
}