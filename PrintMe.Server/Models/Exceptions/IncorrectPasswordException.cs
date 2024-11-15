namespace PrintMe.Server.Models.Exceptions
{
    public class IncorrectPasswordException : Exception
    {
        public IncorrectPasswordException() : base("Your old password is incorrect, please, try again.")
        {
            
        }
    }
}