using Core.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entity.RavenDB
{
    public class User
    {
        public ulong Id { get; set; }

        [MaxLength(32)] public string Name { get; set; }

        public virtual List<Core.Entity.Ban> Bans { get; set; }
        public virtual List<RoleIgnore> IgnoredRoles { get; set; }
    }
}