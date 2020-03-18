namespace Dal.Database.RavenDB.Models
{
    using System.Collections.Generic;

    public class AutoChannel : Automatization
    {
        public string Name { get; set; }

        public List<string> Channels { get; set; }
    }
}