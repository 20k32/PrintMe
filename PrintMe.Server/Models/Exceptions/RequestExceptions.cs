namespace PrintMe.Server.Models.Exceptions;

public class NotFoundRequestInDbException() : Exception("There is no such request in database");
public class NotFoundRequestStatusInDb() : Exception("There is no such request status in database");
public class NotFoundRequestTypeInDb() : Exception("There is no such request type in database");
public class NotFoundRequestStatusReasonInDbException() : Exception("There is no such status reason in database");
public class AlreadyApprovedRequestException() : Exception("Request is already approved");
public class AlreadyDeclinedRequestException() : Exception("Request is already declined");