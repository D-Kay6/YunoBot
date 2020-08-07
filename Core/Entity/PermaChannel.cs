using System.ComponentModel.DataAnnotations;

namespace Core.Entity
{
    public class PermaChannel : Automation
    {
        public PermaChannel()
        {
            Prefix = "👥";
            Name = "{0} channel";
        }

        [Required] [MaxLength(100)] public string Name { get; set; }
    }
}