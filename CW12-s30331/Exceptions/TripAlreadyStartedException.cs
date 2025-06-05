namespace CW12_s30331.Exceptions;

public class TripAlreadyStartedException : Exception
{
    public TripAlreadyStartedException(string? message) : base(message)
    {
    }
}