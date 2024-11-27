namespace PrintMe.Server.Models.Exceptions
{
    public class NoRoleAvailableException : Exception
    {
        public NoRoleAvailableException() : base("There is no such role in database")
        { }
    }
}