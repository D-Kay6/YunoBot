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

        public override bool Equals(object? obj)
        {
            if (obj is DynamicRoleData data)
            {
                return data.RoleId == RoleId &&
                       data.DynamicRoleId == DynamicRoleId;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return RoleId.GetHashCode();
        }
    }
}