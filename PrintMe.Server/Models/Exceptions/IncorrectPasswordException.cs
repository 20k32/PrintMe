namespace PrintMe.Server.Models.Exceptions
{
    public class IncorrectPasswordException : Exception
    {
        public IncorrectPasswordException() : base("Incorrect password.")
        { }
    }
}