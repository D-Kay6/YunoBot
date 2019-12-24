namespace Entity
{
    public class Configuration
    {
        public ulong ClientId;
        public string Token;
        public string DiscordBotsToken;

        public Configuration()
        {
            this.ClientId = 0;
            this.Token = "";
            this.DiscordBotsToken = "";
        }
    }
}
