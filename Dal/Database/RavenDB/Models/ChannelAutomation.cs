namespace Dal.Database.RavenDB.Models
{
    public class ChannelAutomation
    {
        public ChannelAutomation()
        {
            Id = "ChannelAutomations/";
            AutoChannel = new AutoChannel();
            PermaChannel = new PermaChannel();
        }

        public string Id { get; set; }
        public AutoChannel AutoChannel { get; set; }
        public PermaChannel PermaChannel { get; set; }
    }
}