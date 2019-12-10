using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    public class WelcomeMessage
    {
        [Key]
        public ulong ServerId { get; set; }
        public ulong? ChannelId { get; set; }
        public bool UseImage { get; set; }
        [Required, MaxLength(2000)]
        public string Message { get; set; }

        [ForeignKey(nameof(ServerId))]
        public virtual Server Server { get; set; }

        public WelcomeMessage()
        {
            this.UseImage = true;
            this.Message = "Welcome to the party {0}. Hope you will have a good time with us.";
        }
    }
}
