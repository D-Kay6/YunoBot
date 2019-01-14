using Discord;
using System;

namespace Yuno.Logic
{
    [Serializable]
    public class AutoRole : Configuration<AutoRole>
    {
        public bool Enabled { get; private set; }
        protected int PermaRoleIcon;
        protected int AutoRoleIcon;

        public AutoRole(ulong guildId) : base(guildId)
        {
            this.Enabled = true;
            this.AutoRoleIcon = 128126;
            this.PermaRoleIcon = 127918;
        }

        public bool IsAutoRole(IRole role)
        {
            var id = char.ConvertToUtf32(role.Name, 0);
            return AutoRoleIcon.Equals(id);
        }

        public bool IsPermaRole(IRole role)
        {
            var id = char.ConvertToUtf32(role.Name, 0);
            return PermaRoleIcon.Equals(id);
        }

        public string GetAutoRoleIcon()
        {
            return char.ConvertFromUtf32(AutoRoleIcon);
        }

        public string GetPermaRoleIcon()
        {
            return char.ConvertFromUtf32(PermaRoleIcon);
        }

        public void SetAutoRoleIcon(int value)
        {
            AutoRoleIcon = value;
        }

        public void SetAutoRoleIcon(string value)
        {
            AutoRoleIcon = char.ConvertToUtf32(value, 0);
        }

        public void SetPermaRoleIcon(int value)
        {
            PermaRoleIcon = value;
        }

        public void SetPermaRoleIcon(string value)
        {
            PermaRoleIcon = char.ConvertToUtf32(value, 0);
        }

        protected override void Update()
        {
            //Do nothing
        }

        public override void Save()
        {
            Save(this);
        }
    }
}