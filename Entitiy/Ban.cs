using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entity
{
    public class Ban
    {
        [Key]
        public ulong UserId { get; set; }

        public ulong ServerId { get; set; }

        public DateTime? EndDate { get; set; }

        public string Reason { get; set; }


        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        [ForeignKey(nameof(ServerId))]
        public virtual Server Server { get; set; }
    }
}
