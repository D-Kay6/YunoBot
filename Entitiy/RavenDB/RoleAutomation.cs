using System.Collections.Generic;

namespace Entity.RavenDB
{
    public class RoleAutomation
    {
        public RoleAutomation()
        {
            Id = "RoleAutomations/";
            AutoRole = new AutoRole();
            PermaRole = new PermaRole();
            IgnoredUsers = new List<ulong>();
        }

        public string Id { get; set; }
        public AutoRole AutoRole { get; set; }
        public PermaRole PermaRole { get; set; }

        public List<ulong> IgnoredUsers { get; set; }
    }
}