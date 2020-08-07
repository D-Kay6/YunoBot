using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Entity
{
    public class AutoChannel : Automation
    {
        public AutoChannel()
        {
            Prefix = "➕";
            Name = "--channel";
            Channels = new List<GeneratedChannel>();
        }

        [Required] [MaxLength(100)] public string Name { get; set; }

        public virtual List<GeneratedChannel> Channels { get; set; }
    }
}