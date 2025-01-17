namespace PrintMe.Server.Constants;

public static class DbConstants 
{
    public static class RequestStatus
    {
        public const string Pending = "Pending";
        public const string Approved = "Approved";
        public const string Declined = "Declined";

        public static readonly IReadOnlyDictionary<string, int> Dictionary = new Dictionary<string, int>
        {
            { Pending, 1 },
            { Approved, 2 },
            { Declined, 3 }
        };
    }

    public static class PrintOrderStatus
    {
        public const string Pending = "Pending";
        public const string Declined = "Declined";
        public const string Started = "Started";
        public const string Aborted = "Aborted";
        public const string Archived = "Archived";
        public const string Done = "Done";

        public static readonly IReadOnlyDictionary<string, int> Dictionary = new Dictionary<string, int>
        {
            { Pending, 1 },
            { Declined, 2 },
            { Started, 3 },
            { Aborted, 4 },
            { Done, 5 },
            { Archived, 6 }
        };
    }

    public static class UserStatus
    {
        public const string Active = "Active";
        public const string Inactive = "Inactive";
        public const string Blocked = "Blocked";

        public static readonly IReadOnlyDictionary<string, int> Dictionary = new Dictionary<string, int>
        {
            { Active, 1 },
            { Inactive, 2 },
            { Blocked, 3 }
        };
    }

    public static class UserRole
    {
        public const string User = "User";
        public const string PrinterOwner = "PrinterOwner";
        public const string Admin = "Admin";

        public static readonly IReadOnlyDictionary<string, int> Dictionary = new Dictionary<string, int>
        {
            { User, 1 },
            { PrinterOwner, 2 },
            { Admin, 3 }
        };
    }

    public static class PrintOrderStatusReason
    {
        public const string Inappropriate = "Inappropriate";
        public const string OffensiveContent = "OffensiveContent";
        public const string AbsentMaterials = "AbsentMaterials";
        public const string QualityConcerns = "QualityConcerns";

        public static readonly IReadOnlyDictionary<string, int> Dictionary = new Dictionary<string, int>
        {
            { Inappropriate, 1 },
            { OffensiveContent, 2 },
            { AbsentMaterials, 3 },
            { QualityConcerns, 4 }
        };
    }

    public static class RequestStatusReason
    {
        public const string Inappropriate = "Inappropriate";
        public const string OffensiveContent = "OffensiveContent";
        public const string SystemAbuse = "SystemAbuse";

        public static readonly IReadOnlyDictionary<string, int> Dictionary = new Dictionary<string, int>
        {
            { Inappropriate, 1 },
            { OffensiveContent, 2 },
            { SystemAbuse, 3 }
        };
    }

    public static class RequestType
    {
        public const string PrinterApplication = "PrinterApplication";
        public const string PrinterDescriptionChanging = "PrinterDescriptionChanging";
        public const string UserReport = "UserReport";
        public const string AccountDeletion = "AccountDeletion";

        public static readonly IReadOnlyDictionary<string, int> Dictionary = new Dictionary<string, int>
        {
            { PrinterApplication, 1 },
            { PrinterDescriptionChanging, 2 },
            { UserReport, 3 },
            { AccountDeletion, 4 }
        };
    }
}
