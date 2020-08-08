using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity
{
    public class Role
    {
        [Key]
        public ulong Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        //public virtual List<DynamicRole> DynamicRoles { get; set; }
        //public virtual List<ReactionRoleData> ReactionRoles { get; set; }
    }
}