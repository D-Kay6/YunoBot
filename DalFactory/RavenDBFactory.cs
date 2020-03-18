namespace DalFactory
{
    using Dal.Database.RavenDB.Repositories;
    using IDal.Database.Raven;

    public class RavenDBFactory
    {
        public static IDbServer GenerateServer()
        {
            return new ServerRepository();
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