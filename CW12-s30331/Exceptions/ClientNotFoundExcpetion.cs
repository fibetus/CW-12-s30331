namespace CW12_s30331.Exceptions;

public class ClientNotFoundExcpetion : Exception
{
    public ClientNotFoundExcpetion(string? message) : base(message)
    {
    }
}