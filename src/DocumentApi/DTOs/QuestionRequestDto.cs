namespace DocumentApi.DTOs;

public record QuestionRequestDto
{
    public required string Question { get; set; }
}