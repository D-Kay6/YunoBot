using System.Collections.Generic;

namespace Entity.RavenDB
{
    public class Localization
    {
        public List<string> GroupPraises;
        public Dictionary<string, string> Messages;
        public List<string> UserPraises;

        public Localization()
        {
            Messages = new Dictionary<string, string>();
            UserPraises = new List<string>();
            GroupPraises = new List<string>();
        }
    }
}