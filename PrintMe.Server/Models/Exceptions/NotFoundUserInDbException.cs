namespace PrintMe.Server.Models.Exceptions
{
    public class NotFoundUserInDbException : Exception
    {
        public NotFoundUserInDbException() : base("There is no such user in database")
        { }
    }
}