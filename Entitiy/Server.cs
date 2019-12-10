using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    public class Server
    {
        [Key]
        public ulong Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public virtual LanguageSetting LanguageSetting { get; set; }

        [Required]
        public virtual CommandSetting CommandSetting { get; set; }

        [Required]
        public virtual WelcomeMessage WelcomeMessage { get; set; }

        [Required]
        public virtual AutoChannel AutoChannel { get; set; }

        [Required]
        public virtual PermaChannel PermaChannel { get; set; }

        [Required]
        public virtual AutoRole AutoRole { get; set; }

        [Required]
        public virtual PermaRole PermaRole { get; set; }

        public virtual List<GeneratedChannel> GeneratedChannels { get; set; }

        public virtual List<RoleIgnore> IgnoredUsers { get; set; }

        public Server()
        {
            this.LanguageSetting = new LanguageSetting();
            this.CommandSetting = new CommandSetting();
            this.WelcomeMessage = new WelcomeMessage();
            this.AutoChannel = new AutoChannel();
            this.PermaChannel = new PermaChannel();
            this.AutoRole = new AutoRole();
            this.PermaRole = new PermaRole();
            this.IgnoredUsers = new List<RoleIgnore>();
        }
    }
}
