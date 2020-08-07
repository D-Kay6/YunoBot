using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity
{
    public class CustomCommand
    {
        [Key] [Column(Order = 0)] public ulong ServerId { get; set; }

        [Key]
        [Column(Order = 1)]
        [MaxLength(50)]
        public string Command { get; set; }

        [Required] public string Response { get; set; }

        [ForeignKey(nameof(ServerId))] public virtual Server Server { get; set; }
    }
}