using System.Collections.Generic;

namespace Dal.Database.RavenDB.Models
{
    public class AutoChannel : Automatization
    {
        public string Name { get; set; }

        public List<string> Channels { get; set; }
    }
}