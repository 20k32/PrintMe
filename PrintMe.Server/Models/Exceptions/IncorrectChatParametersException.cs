namespace PrintMe.Server.Models.Exceptions;

public class IncorrectChatParametersException : Exception
{
    public IncorrectChatParametersException() : base("Incorrect chat parameters.")
    { }
}