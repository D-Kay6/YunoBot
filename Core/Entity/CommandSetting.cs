using System.Collections.Generic;

namespace Entity.RavenDB
{
    public class CommandSetting
    {
        public string Prefix { get; set; }
        public Dictionary<string, string> CustomResponses { get; set; }

        public CommandSetting()
        {
            this.Prefix = "/";
            this.CustomResponses = new Dictionary<string, string>();
        }
    }
}