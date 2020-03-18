namespace Core.Entity
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class RoleIgnore
    {
        [Key] [Column(Order = 0)] public ulong ServerId { get; set; }

        [Key] [Column(Order = 1)] public ulong UserId { get; set; }

        [ForeignKey(nameof(ServerId))] public virtual Server Server { get; set; }

        [ForeignKey(nameof(UserId))] public virtual User User { get; set; }
    }
}