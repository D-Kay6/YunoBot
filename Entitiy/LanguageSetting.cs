using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    public class LanguageSetting
    {
        [Key]
        public ulong ServerId { get; set; }

        [Required]
        public Language Language { get; set; }

        [ForeignKey(nameof(ServerId))]
        public virtual Server Server { get; set; }

        public LanguageSetting()
        {
            this.Language = Language.English;
        }
    }
}
