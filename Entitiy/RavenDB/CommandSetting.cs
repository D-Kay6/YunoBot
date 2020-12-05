using System.Collections.Generic;

namespace Entity.RavenDB
{
    public class CommandSetting
    {
        public CommandSetting()
        {
            Prefix = "/";
            CustomResponses = new Dictionary<string, string>();
        }

        public string Prefix { get; set; }
        public Dictionary<string, string> CustomResponses { get; set; }
    }
}