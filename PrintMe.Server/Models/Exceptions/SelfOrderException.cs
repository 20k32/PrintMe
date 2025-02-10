namespace PrintMe.Server.Models.Exceptions;

public class SelfOrderException() : Exception("You can't order from yourself") { };