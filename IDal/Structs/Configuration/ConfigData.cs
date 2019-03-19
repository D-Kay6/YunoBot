namespace IDal.Structs.Configuration
{
    public class ConfigData
    {
        public ulong ClientId;
        public string Token;
        public string DiscordBotsToken;
        public string Prefix;

        public ConfigData()
        {
            this.ClientId = 0;
            this.Token = "";
            this.DiscordBotsToken = "";
            this.Prefix = "/";
        }
    }
}
