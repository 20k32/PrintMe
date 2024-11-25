namespace PrintMe.Server.Models.Exceptions
{
    public class NotFoundRequestInDbException() : Exception("There is no such request in database");
}