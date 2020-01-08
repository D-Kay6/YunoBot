#if DEBUG
using Dal.EF.SingleThreaded;
#else
using Dal.EF.MultiThreaded;
#endif
using IDal.Database;

namespace DalFactory
{
    public class DatabaseFactory
    {
        public static IDbServer GenerateServer()
        {
            return new ServerRepository();
        }

        public static IDbUser GenerateUser()
        {
            return new UserRepository();
        }

        public static IDbBan GenerateBan()
        {
            return new BanRepository();
        }

        public static IDbLanguage GenerateLanguage()
        {
            return new LanguageRepository();
        }

        public static IDbCommand GenerateCommand()
        {
            return new CommandRepository();
        }

        public static IDbWelcome GenerateWelcome()
        {
            return new WelcomeSettingsRepository();
        }

        public static IDbChannel GenerateChannel()
        {
            return new ChannelRepository();
        }

        public static IDbRole GenerateRole()
        {
            return new RoleRepository();
        }
    }
}
