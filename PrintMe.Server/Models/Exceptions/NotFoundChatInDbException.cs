namespace PrintMe.Server.Models.Exceptions;

public class NotFoundChatInDbException : Exception
{
    public NotFoundChatInDbException() : base("Chat with this id not found")
    { }
}

public class FoundChatInDbException : Exception
{
    public FoundChatInDbException() : base("Such chat already exists in database")
    { }
}