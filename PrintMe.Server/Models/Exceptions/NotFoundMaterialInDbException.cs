namespace PrintMe.Server.Models.Exceptions
{
    public class NotFoundMaterialInDbException : Exception
    {
        public NotFoundMaterialInDbException() : base("There are no materials in the database.")
        { }
    }
}