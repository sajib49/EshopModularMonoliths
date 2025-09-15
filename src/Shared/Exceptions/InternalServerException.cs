namespace Shared.Exceptions;

public class InternalServerException : Exception
{
    public InternalServerException(string message)
    {
        
    }

    public InternalServerException(string message, string details)
        : base(message)
    {
        Details = details;
    }

    public string? Details { get; set; }
}
