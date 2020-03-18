namespace Core.Entity
{
    using System.ComponentModel.DataAnnotations;

    public class PermaChannel : Automatization
    {
        public PermaChannel()
        {
            Prefix = "👥";
            Name = "{0} channel";
        }

        [Required] [MaxLength(100)] public string Name { get; set; }
    }
}