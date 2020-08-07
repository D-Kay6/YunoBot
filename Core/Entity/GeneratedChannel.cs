using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity
{
    public class GeneratedChannel
    {
        [Key] [Column(Order = 0)] public ulong ServerId { get; set; }

        [Key] [Column(Order = 1)] public ulong ChannelId { get; set; }

        [ForeignKey(nameof(ServerId))] public virtual AutoChannel AutoChannel { get; set; }

        [ForeignKey(nameof(ServerId))] public virtual Server Server { get; set; }
    }
}