namespace PrintMe.Server.Logic
{
    public static class Roles
    {
        public const string USER = "User";
        public const string PRINTER_OWNER = "Printer owner";
        public const string ADMIN = "Admin";

        public static bool IsRolePresent(string role)
            => role.Equals(USER, StringComparison.OrdinalIgnoreCase)
               || role.Equals(PRINTER_OWNER, StringComparison.OrdinalIgnoreCase)
               || role.Equals(ADMIN, StringComparison.OrdinalIgnoreCase);
    }
}