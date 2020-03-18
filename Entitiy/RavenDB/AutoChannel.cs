namespace Entity.RavenDB
{
    using System.Collections.Generic;

    public class AutoChannel : Automation
    {
        public AutoChannel()
        {
            Enabled = true;
            Prefix = "➕";
            Name = "--channel";
            Channels = new List<ulong>();
        }

        public string Name { get; set; }

        public List<ulong> Channels { get; set; }
    }
}