namespace PrintMe.Server.Models.Exceptions
{
    public class DatabaseInternalException : Exception
    {
        public DatabaseInternalException() : base("Internal server error in database.")
        { }

        public DatabaseInternalException(Exception ex) : base(ex.Message, ex)
        { }
    }
}