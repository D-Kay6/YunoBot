using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Entity
{
    public class User
    {
        public ulong Id { get; set; }

        [MaxLength(32)] public string Name { get; set; }

        public virtual List<Ban> Bans { get; set; }
        public virtual List<RoleIgnore> IgnoredRoles { get; set; }
    }
}