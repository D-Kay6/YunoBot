using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
