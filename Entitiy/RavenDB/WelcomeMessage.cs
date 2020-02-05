namespace Entity.RavenDB
{
    public class WelcomeMessage
    {
        public ulong? ChannelId { get; set; }
        public bool UseImage { get; set; }
        public string Message { get; set; }

        public WelcomeMessage()
        {
            this.UseImage = true;
            this.Message = "Welcome to the party {0}. Hope you will have a good time with us.";
        }
    }
}