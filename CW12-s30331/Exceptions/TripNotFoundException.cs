namespace CW12_s30331.Exceptions;

public class TripNotFoundException : Exception
{
    public TripNotFoundException(string? message) : base(message)
    {
    }
}