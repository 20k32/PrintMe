namespace PrintMe.Server.Models.Exceptions;

public class ChatAlreadyArchivedException : Exception
{
    public ChatAlreadyArchivedException() : base("Chat is already archived")
    {

    }
}