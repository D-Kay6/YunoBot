namespace Core.Entity
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public abstract class Automatization
    {
        [Key] [Column(Order = 0)] public ulong ServerId { get; set; }

        [Required] [MaxLength(100)] public string Prefix { get; set; }

        public bool Enabled { get; set; }

        [ForeignKey(nameof(ServerId))] public virtual Server Server { get; set; }
    }
}