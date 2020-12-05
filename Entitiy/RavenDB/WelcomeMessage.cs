namespace Entity.RavenDB
{
    public class WelcomeMessage
    {
        public WelcomeMessage()
        {
            UseImage = true;
            Message = "Welcome to the party {0}. Hope you will have a good time with us.";
        }

        public ulong? ChannelId { get; set; }
        public bool UseImage { get; set; }
        public string Message { get; set; }
    }
}