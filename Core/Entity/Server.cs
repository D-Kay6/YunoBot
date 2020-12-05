using Core.Enum;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Entity
{
    public class Server
    {
        [Key]
        public ulong Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public virtual LanguageSetting LanguageSetting { get; set; }

        [Required]
        public virtual CommandSetting CommandSetting { get; set; }

        [Required]
        public virtual WelcomeMessage WelcomeMessage { get; set; }

        public virtual List<DynamicChannel> DynamicChannels { get; set; }
        public virtual List<DynamicRole> DynamicRoles { get; set; }
        public virtual List<ReactionRole> ReactionRoles { get; set; }
        public virtual List<GeneratedChannel> GeneratedChannels { get; set; }
        public virtual List<DynamicRoleIgnore> IgnoredUsers { get; set; }

        public Server()
        {
            LanguageSetting = new LanguageSetting();
            CommandSetting = new CommandSetting();
            WelcomeMessage = new WelcomeMessage();

            DynamicChannels = new List<DynamicChannel>(2)
            {
                new DynamicChannel(AutomationType.Temporary),
                new DynamicChannel(AutomationType.Permanent)
            };
            DynamicRoles = new List<DynamicRole>();
            ReactionRoles = new List<ReactionRole>();
            GeneratedChannels = new List<GeneratedChannel>();
            IgnoredUsers = new List<DynamicRoleIgnore>();
        }
    }
}