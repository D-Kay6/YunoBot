using System.Collections.Generic;

namespace Dal.Database.RavenDB.Models
{
    public class RoleAutomation
    {
        public string Id { get; set; }
        public AutoRole AutoRole { get; set; }
        public PermaRole PermaRole { get; set; }

        public List<string> IgnoredUsers { get; set; }
    }
}