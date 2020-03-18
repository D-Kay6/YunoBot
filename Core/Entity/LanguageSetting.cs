namespace Core.Entity
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Enum;

    public class LanguageSetting
    {
        public LanguageSetting()
        {
            Language = Language.English;
        }

        [Key] public ulong ServerId { get; set; }

        [Required] public Language Language { get; set; }

        [ForeignKey(nameof(ServerId))] public virtual Server Server { get; set; }
    }
}