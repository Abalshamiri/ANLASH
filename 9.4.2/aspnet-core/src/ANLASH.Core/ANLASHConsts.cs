using ANLASH.Debugging;

namespace ANLASH
{
    public class ANLASHConsts
    {
        public const string LocalizationSourceName = "ANLASH";

        public const string ConnectionStringName = "Default";

        public const bool MultiTenancyEnabled = true;


        /// <summary>
        /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
        /// </summary>
        public static readonly string DefaultPassPhrase =
            DebugHelper.IsDebug ? "gsKxGZ012HLL3MI5" : "dc3854521a434cb4b78df66eb2370c8b";
    }
}
