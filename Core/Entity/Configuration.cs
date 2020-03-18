namespace Core.Entity
{
    public class Configuration
    {
        public ulong ClientId;
        public string DiscordBotsToken;
        public string SpotifyId;
        public string SpotifySecret;
        public string Token;

        public Configuration()
        {
            ClientId = 0;
            Token = "";
            DiscordBotsToken = "";
            SpotifyId = "";
            SpotifySecret = "";
        }
    }
}