namespace PrintMe.Server.Models.Extensions
{
    public static class ConfigurationExtensions
    {
        private const string MAX_ENTRIES_IN_RESPONSE_KEY = "MaxPrintersReturnedByQuery";

        public static int GetMaxPrintersInResponse(this IConfiguration configuration) =>
            int.Parse(configuration[MAX_ENTRIES_IN_RESPONSE_KEY]!);
    }
}