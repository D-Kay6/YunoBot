namespace Entity.RavenDB
{
    using System.Collections.Generic;

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