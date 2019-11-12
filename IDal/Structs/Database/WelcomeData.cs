namespace IDal.Structs.Database
{
    public class WelcomeData
    {
        public ulong? ChannelId { get; private set; }
        public bool UseImage { get; private set; }
        public string Message { get; private set; }

        public WelcomeData(ulong? channelId, bool useImage, string message)
        {
            this.ChannelId = channelId;
            this.UseImage = useImage;
            this.Message = message;
        }
    }
}
