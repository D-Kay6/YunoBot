using System.Collections.Generic;

namespace Entity.RavenDB
{
    public class RoleAutomation
    {
        public string Id { get; set; }
        public AutoRole AutoRole { get; set; }
        public PermaRole PermaRole { get; set; }

        public List<ulong> IgnoredUsers { get; set; }

        public RoleAutomation()
        {
            this.Id = "RoleAutomations/";
            this.AutoRole = new AutoRole();
            this.PermaRole = new PermaRole();
            this.IgnoredUsers = new List<ulong>();
        }
    }
}