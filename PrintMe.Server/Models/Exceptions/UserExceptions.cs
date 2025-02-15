namespace PrintMe.Server.Models.Exceptions;

public class IncorrectPasswordException() : Exception("Incorrect password.");
public class InvalidEmailFormatException() : Exception("Email format is invalid.");
public class NotFoundUserInDbException() : Exception("There is no such user in database");
public class InvalidUUIDTokenException() : Exception("Token is invalid.");