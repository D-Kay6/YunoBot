using Dal.MySql;
using IDal.Interfaces.Database;

namespace DalFactory
{
    public class DatabaseFactory
    {
        public static IServerSettings GenerateServerSettings()
        {
            return new ServerSettings();
        }

        public static IAutoChannel GenerateAutoChannel()
        {
            return new AutoChannel();
        }

        public static IAutoRole GenerateAutoRole()
        {
            return new AutoRole();
        }

        public static IWelcomeMessage GenerateWelcomeMessage()
        {
            return new WelcomeMessage();
        }
    }
}
