using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity
{
    public class ReactionRole
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong Id { get; set; }

        public ulong ServerId { get; set; }

        public ulong Message { get; set; }

        [Required]
        [MaxLength(32)]
        public string Reaction { get; set; }

        [ForeignKey(nameof(ServerId))]
        public virtual Server Server { get; set; }

        [ForeignKey("Roles")]
        public virtual List<Role> Roles { get; set; }
    }
}