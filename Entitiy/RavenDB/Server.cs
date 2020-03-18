namespace Entity.RavenDB
{
    public class Server
    {
        public Server()
        {
            Id = "Servers/";
            LanguageSetting = new LanguageSetting();
            CommandSetting = new CommandSetting();
            WelcomeMessage = new WelcomeMessage();
        }

        public string Id { get; set; }
        public ulong ServerId { get; set; }
        public string Name { get; set; }
        public string ChannelAutomation { get; set; }
        public string RoleAutomation { get; set; }
        public LanguageSetting LanguageSetting { get; set; }
        public CommandSetting CommandSetting { get; set; }
        public WelcomeMessage WelcomeMessage { get; set; }
    }
}