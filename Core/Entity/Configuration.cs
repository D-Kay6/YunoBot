namespace Entity.RavenDB
{
    public class Configuration
    {
        public ulong ClientId;
        public string Token;
        public string DiscordBotsToken;
        public string SpotifyId;
        public string SpotifySecret;

        public Configuration()
        {
            this.ClientId = 0;
            this.Token = "";
            this.DiscordBotsToken = "";
            this.SpotifyId = "";
            this.SpotifySecret = "";
        }
    }
}