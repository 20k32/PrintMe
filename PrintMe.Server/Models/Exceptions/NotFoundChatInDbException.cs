namespace PrintMe.Server.Models.Exceptions;

public class NotFoundChatInDbException : Exception
{
    public NotFoundChatInDbException() : base("Chat with this id not found")
    { }
}