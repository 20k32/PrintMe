namespace PrintMe.Server.Models.Exceptions
{
    public class NotFoundOrderInDbException : Exception
    {
        public NotFoundOrderInDbException() : base("There is no such order in database")
        { }
        
        public NotFoundOrderInDbException(Exception inner) : base("There is no such order in database", inner)
        { } 
    }
}