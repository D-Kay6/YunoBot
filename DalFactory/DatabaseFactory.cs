using Dal.Database.MySql.EF.Repositories;
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

        public static IDbCommandSetting GenerateCommandSetting()
        {
            return new CommandSettingRepository();
        }

        public static IDbCommandCustom GenerateCommandCustom()
        {
            return new CustomCommandRepository();
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