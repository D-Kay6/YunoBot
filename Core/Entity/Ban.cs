namespace Core.Entity
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Ban
    {
        [Key] [Column(Order = 0)] public ulong UserId { get; set; }

        [Key] [Column(Order = 1)] public ulong ServerId { get; set; }

        public DateTime? EndDate { get; set; }

        public string Reason { get; set; }


        [ForeignKey(nameof(UserId))] public virtual User User { get; set; }

        [ForeignKey(nameof(ServerId))] public virtual Server Server { get; set; }
    }
}