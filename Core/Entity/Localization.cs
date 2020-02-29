using System.Collections.Generic;

namespace Core.Entity
{
    public class Localization
    {
        public Dictionary<string, string> Messages;
        public List<string> UserPraises;
        public List<string> GroupPraises;

        public Localization()
        {
            this.Messages = new Dictionary<string, string>();
            this.UserPraises = new List<string>();
            this.GroupPraises = new List<string>();
        }
    }
}