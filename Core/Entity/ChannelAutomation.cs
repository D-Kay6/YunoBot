namespace Entity.RavenDB
{
    public class ChannelAutomation
    {
        public string Id { get; set; }
        public AutoChannel AutoChannel { get; set; }
        public PermaChannel PermaChannel { get; set; }

        public ChannelAutomation()
        {
            this.Id = "ChannelAutomations/";
            this.AutoChannel = new AutoChannel();
            this.PermaChannel = new PermaChannel();
        }
    }
}