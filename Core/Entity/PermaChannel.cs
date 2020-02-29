using System.ComponentModel.DataAnnotations;

namespace Core.Entity
{
    public class PermaChannel : Automatization
    {
        [Required, MaxLength(100)]
        public string Name { get; set; }

        public PermaChannel()
        {
            this.Prefix = "👥";
            this.Name = "{0} channel";
        }
    }
}