using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entity
{
    public class AutoChannel : Automatization
    {
        [Required, MaxLength(100)]
        public string Name { get; set; }

        public virtual List<GeneratedChannel> Channels { get; set; }

        public AutoChannel()
        {
            this.Prefix = "➕";
            this.Name = "--channel";
            this.Channels = new List<GeneratedChannel>();
        }
    }
}
