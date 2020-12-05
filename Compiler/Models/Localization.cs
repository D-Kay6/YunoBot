using System.Collections.Generic;

namespace Compiler.Models
{
    public class Localization
    {
        public Dictionary<string, object> Messages { get; set; }
        public List<string> GroupPraises { get; set; }
        public List<string> UserPraises { get; set; }
    }
}