using System.Collections.Generic;

namespace Entity.RavenDB
{
    public class Server
    {
        public string Id { get; set; }
        public ulong ServerId { get; set; }
        public string Name { get; set; }
        public string ChannelAutomation { get; set; }
        public string RoleAutomation { get; set; }
        public LanguageSetting LanguageSetting { get; set; }
        public CommandSetting CommandSetting { get; set; }
        public WelcomeMessage WelcomeMessage { get; set; }

        public Server()
        {
            this.Id = "Servers/";
            this.LanguageSetting = new LanguageSetting();
            this.CommandSetting = new CommandSetting();
            this.WelcomeMessage = new WelcomeMessage();
        }
    }
}