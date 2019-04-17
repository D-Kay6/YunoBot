using Discord;
using System;

namespace Logic.Data
{
    [Serializable]
    public class AutoRole : Configuration<AutoRole>
    {
        public string AutoPrefix { get; protected set; }
        public string PermaPrefix { get; protected set; }

        public AutoRole(ulong guildId) : base(guildId)
        {
            Enabled = true;
            AutoPrefix = "👾";
            PermaPrefix = "🎮";
        }

        public bool Enabled { get; }

        public bool IsAutoRole(IRole role)
        {
            return role.Name.StartsWith(AutoPrefix);
        }

        public bool IsPermaRole(IRole role)
        {
            return role.Name.StartsWith(PermaPrefix);
        }

        public void SetAutoRoleIcon(string value)
        {
            AutoPrefix = value;
        }

        public void SetPermaRoleIcon(string value)
        {
            PermaPrefix = value;
        }
        
        public override void Save()
        {
            base.Save(this);
        }

        protected int AutoRoleIcon;
        protected int PermaRoleIcon;

        protected override void Update()
        {
            if (AutoPrefix == null) AutoPrefix = char.ConvertFromUtf32(AutoRoleIcon);
            if (PermaPrefix == null) PermaPrefix = char.ConvertFromUtf32(PermaRoleIcon);
        }
    }
}