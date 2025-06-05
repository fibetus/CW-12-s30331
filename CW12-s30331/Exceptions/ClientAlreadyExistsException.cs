namespace CW12_s30331.Exceptions;

public class ClientAlreadyExistsException : Exception
{
    public ClientAlreadyExistsException(string? message) : base(message)
    {
    }
}