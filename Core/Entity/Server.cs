namespace Core.Entity
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Server
    {
        public Server()
        {
            LanguageSetting = new LanguageSetting();
            CommandSetting = new CommandSetting();
            WelcomeMessage = new WelcomeMessage();
            AutoChannel = new AutoChannel();
            PermaChannel = new PermaChannel();
            AutoRole = new AutoRole();
            PermaRole = new PermaRole();
            IgnoredUsers = new List<RoleIgnore>();
        }

        [Key] public ulong Id { get; set; }

        [Required] [MaxLength(100)] public string Name { get; set; }

        [Required] public virtual LanguageSetting LanguageSetting { get; set; }

        [Required] public virtual CommandSetting CommandSetting { get; set; }

        [Required] public virtual WelcomeMessage WelcomeMessage { get; set; }

        [Required] public virtual AutoChannel AutoChannel { get; set; }

        [Required] public virtual PermaChannel PermaChannel { get; set; }

        [Required] public virtual AutoRole AutoRole { get; set; }

        [Required] public virtual PermaRole PermaRole { get; set; }

        public virtual List<CustomCommand> CustomCommands { get; set; }

        public virtual List<GeneratedChannel> GeneratedChannels { get; set; }

        public virtual List<RoleIgnore> IgnoredUsers { get; set; }
    }
}