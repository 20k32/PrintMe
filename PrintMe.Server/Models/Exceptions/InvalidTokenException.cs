namespace PrintMe.Server.Models.Exceptions;

public class InvalidTokenException : Exception
{
    public InvalidTokenException() : base("Invalid token")
    { }
}