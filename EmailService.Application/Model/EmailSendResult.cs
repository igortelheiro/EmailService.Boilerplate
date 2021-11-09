using Common.Api.DTOs;

namespace EmailService.Application.Model;

public record EmailSendResult : ResponseDto
{
    public bool Success => Error == null;
    public string Error { get; init; }
}
