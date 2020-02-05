namespace Dal.Database.RavenDB.Models
{
    public class WelcomeMessage
    {
        public string ChannelId { get; set; }
        public bool UseImage { get; set; }
        public string Message { get; set; }

        public WelcomeMessage()
        {
            this.UseImage = true;
            this.Message = "Welcome to the party {0}. Hope you will have a good time with us.";
        }
    }
}