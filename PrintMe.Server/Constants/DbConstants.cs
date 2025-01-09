namespace PrintMe.Server.Constants;

public static class DbConstants 
{
    public static class RequestStatus
    {
        public const string Pending = "Pending";
        public const string Approved = "Approved"; 
        public const string Declined = "Declined";
    }

    public static class RequestType
    {
        public const string PrinterApplication = "PrinterApplication";
        public const string PrinterDescriptionChanging = "PrinterDescriptionChanging";
        public const string UserReport = "UserReport";
        public const string AccountDeletion = "AccountDeletion";
    }

    public static class UserRole
    {
        public const string User = "User";
        public const string PrinterOwner = "PrinterOwner";
        public const string Admin = "Admin";
    }

    public static class UserStatus 
    {
        public const string Active = "Active";
        public const string Inactive = "Inactive";
        public const string Blocked = "Blocked";
    }

    public static class PrintOrderStatusReason
    {
        public const string Inappropriate = "Inappropriate";
        public const string OffensiveContent = "OffensiveContent";
        public const string AbsentMaterials = "AbsentMaterials";
        public const string QualityConcerns = "QualityConcerns";
    }

    public static class PrintOrderStatus
    {
        public const string Pending = "Pending";
        public const string Declined = "Declined";
        public const string Started = "Started";
        public const string Aborted = "Aborted";
        public const string Archived = "Archived";
    }

    public static class RequestStatusReason 
    {
        public const string Inappropriate = "Inappropriate";
        public const string OffensiveContent = "OffensiveContent";
        public const string SystemAbuse = "SystemAbuse";
    }
}
