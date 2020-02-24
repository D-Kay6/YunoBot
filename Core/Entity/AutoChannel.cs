using System.Collections.Generic;

namespace Entity.RavenDB
{
    public class AutoChannel : Automatization
    {
        public string Name { get; set; }

        public List<ulong> Channels { get; set; }

        public AutoChannel()
        {
            this.Enabled = true;
            this.Prefix = "➕";
            this.Name = "--channel";
            this.Channels = new List<ulong>();
        }
    }
}