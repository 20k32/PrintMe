namespace PrintMe.Server.Models.Exceptions
{
    public class NotFoundPrinterInDbException : Exception
    {
        public NotFoundPrinterInDbException() : base("Such printer not found in database")
        { }
    }
}