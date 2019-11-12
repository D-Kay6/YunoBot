using System.Collections.Generic;

namespace IDal.Structs.Localization
{
    public class LanguageData
    {
        public Dictionary<string, string> Messages;
        public List<string> UserPraises;
        public List<string> GroupPraises;

        public LanguageData()
        {
            this.Messages = new Dictionary<string, string>();
            this.UserPraises = new List<string>();
            this.GroupPraises = new List<string>();
        }
    }
}
