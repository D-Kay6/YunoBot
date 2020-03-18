namespace DalFactory
{
    using Dal.Database.MySql.EF.Repositories;
    using IDal.Database;

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
            return new AutoChannelRepository();
        }

        public static IDbAutoRole GenerateRole()
        {
            return new RoleRepository();
        }
    }
}