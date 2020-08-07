using Entity.RavenDB;

namespace Dal.Database.RavenDB.Models
{
    public class Server
    {
        public string Id { get; set; }
        public string ServerId { get; set; }
        public string Name { get; set; }
        public string ChannelAutomation { get; set; }
        public string RoleAutomation { get; set; }
        public LanguageSetting LanguageSetting { get; set; }
        public CommandSetting CommandSetting { get; set; }
        public WelcomeMessage WelcomeMessage { get; set; }
    }
}