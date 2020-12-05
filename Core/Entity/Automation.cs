using Core.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity
{
    public abstract class Automation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong Id { get; set; }

        public ulong ServerId { get; set; }

        public AutomationType Type { get; set; }

        [ForeignKey(nameof(ServerId))]
        public virtual Server Server { get; set; }
    }
}