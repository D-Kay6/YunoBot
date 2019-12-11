using Dal.EF;
using Dal.MySql;
using IDal.Interfaces.Database;

namespace DalFactory
{
    public class DatabaseFactory
    {
        public static IServer GenerateServer()
        {
            return new ServerRepository();
        }

        public static ILanguage GenerateLanguage()
        {
            return new LanguageRepository();
        }

        public static ICommand GenerateCommand()
        {
            return new CommandRepository();
        }

        public static IWelcome GenerateWelcome()
        {
            return new WelcomeSettingsRepository();
        }

        public static IChannel GenerateChannel()
        {
            return new ChannelRepository();
        }

        public static IRole GenerateRole()
        {
            return new RoleRepository();
        }
    }
}
