using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity
{
    public class DynamicRoleData
    {
        [Key]
        [Column(Order = 0)]
        public ulong DynamicRoleId { get; set; }

        [Key]
        [Column(Order = 1)]
        public ulong RoleId { get; set; }

        [ForeignKey(nameof(DynamicRoleId))]
        public virtual DynamicRole DynamicRole { get; set; }
    }
}