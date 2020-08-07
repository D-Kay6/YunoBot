using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity
{
    public class CommandSetting
    {
        public CommandSetting()
        {
            Prefix = "/";
        }

        [Key] public ulong ServerId { get; set; }

        [Required] [MaxLength(20)] public string Prefix { get; set; }

        [ForeignKey(nameof(ServerId))] public virtual Server Server { get; set; }
    }
}