using PrintMe.Server.Constants;

namespace PrintMe.Server.Logic
{
    public static class Roles
    {
        public static bool IsRolePresent(string role)
            => role.Equals(DbConstants.UserRole.User, StringComparison.OrdinalIgnoreCase)
               || role.Equals(DbConstants.UserRole.PrinterOwner, StringComparison.OrdinalIgnoreCase)
               || role.Equals(DbConstants.UserRole.Admin, StringComparison.OrdinalIgnoreCase);
    }
}