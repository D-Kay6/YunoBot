namespace Dal.Database.RavenDB.Models
{
    using System.Collections.Generic;

    public class RoleAutomation
    {
        public string Id { get; set; }
        public AutoRole AutoRole { get; set; }
        public PermaRole PermaRole { get; set; }

        public List<string> IgnoredUsers { get; set; }
    }
}