using Core.Enum;
using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Entity
{
    public class DynamicChannel : Automation
    {
        [Required]
        [MaxLength(100)]
        public string Prefix { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public DynamicChannel(AutomationType type)
        {
            switch (type)
            {
                case AutomationType.Temporary:
                    Type = type;
                    Prefix = "➕";
                    Name = "--channel";
                    break;
                case AutomationType.Permanent:
                    Type = type;
                    Prefix = "👥";
                    Name = "{0} channel";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}