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

        public static IDbAutoChannel GenerateAutoChannel()
        {
            return new AutoChannelRepository();
        }

        public static IDbGeneratedChannel GenerateGeneratedChannel()
        {
            return new GeneratedChannelRepository();
        }

        public static IDbPermaChannel GeneratePermaChannel()
        {
            return new PermaChannelRepository();
        }

        public static IDbAutoRole GenerateAutoRole()
        {
            return new AutoRoleRepository();
        }

        public static IDbPermaRole GeneratePermaRole()
        {
            return new PermaRoleRepository();
        }

        public static IDbRoleIgnore GenerateRoleIgnore()
        {
            return new RoleIgnoreRepository();
        }
    }
}