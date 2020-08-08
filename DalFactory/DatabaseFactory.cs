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

        public static IDbRole GenerateRole()
        {
            return new RoleRepository();
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

        public static IDbDynamicChannel GenerateDynamicChannel()
        {
            return new DynamicChannelRepository();
        }

        public static IDbGeneratedChannel GenerateGeneratedChannel()
        {
            return new GeneratedChannelRepository();
        }

        public static IDbDynamicRole GenerateDynamicRole()
        {
            return new DynamicRoleRepository();
        }

        public static IDbDynamicRoleData GenerateDynamicRoleData()
        {
            return new DynamicRoleDataRepository();
        }

        public static IDbRoleIgnore GenerateRoleIgnore()
        {
            return new RoleIgnoreRepository();
        }

        public static IDbReactionRole GenerateReactionRole()
        {
            return new ReactionRoleRepository();
        }

        public static IDbReactionRoleData GenerateReactionRoleData()
        {
            return new ReactionRoleDataRepository();
        }
    }
}