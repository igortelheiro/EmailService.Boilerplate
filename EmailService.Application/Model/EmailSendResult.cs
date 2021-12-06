namespace EmailService.Application.Model;

public record EmailSendResult
{
    public bool Success => Error == null;
    public string Error { get; init; }
}
