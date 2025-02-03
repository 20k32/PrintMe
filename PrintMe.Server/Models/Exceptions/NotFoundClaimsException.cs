namespace PrintMe.Server.Models.Exceptions;

public class NotFoundClaimsException : Exception
{
    public NotFoundClaimsException() : base("Claims not found")
    { }
}