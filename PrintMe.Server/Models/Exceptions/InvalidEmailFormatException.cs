namespace PrintMe.Server.Models.Exceptions;

public class InvalidEmailFormatException : Exception
{
    public InvalidEmailFormatException() : base("Email format is invalid.")
    {
        
    }
}