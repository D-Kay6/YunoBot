using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity
{
    public class ReactionRoleData
    {
        [Key]
        [Column(Order = 0)]
        public ulong ReactionRoleId { get; set; }

        [Key]
        [Column(Order = 1)]
        public ulong RoleId { get; set; }

        [ForeignKey(nameof(ReactionRoleId))]
        public virtual ReactionRole ReactionRole { get; set; }
    }
}