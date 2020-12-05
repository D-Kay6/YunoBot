using Core.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity
{
    public class LanguageSetting
    {
        [Key]
        public ulong ServerId { get; set; }

        public Language Language { get; set; }

        [ForeignKey(nameof(ServerId))] public virtual Server Server { get; set; }

        public LanguageSetting()
        {
            Language = Language.English;
        }
    }
}