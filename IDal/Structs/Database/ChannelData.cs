namespace IDal.Structs.Database
{
    public class ChannelData
    {
        public string AutoPrefix { get; private set; }
        public string AutoName { get; private set; }
        public string PermaPrefix { get; private set; }
        public string PermaName { get; private set; }

        public ChannelData(string autoPrefix, string autoName, string permaPrefix, string permaName)
        {
            this.AutoPrefix = autoPrefix;
            this.AutoName = autoName;
            this.PermaPrefix = permaPrefix;
            this.PermaName = permaName;
        }
    }
}
