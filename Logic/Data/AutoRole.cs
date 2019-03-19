using Discord;
using System;

namespace Logic.Data
{
    [Serializable]
    public class AutoRole : Configuration<AutoRole>
    {
        protected int AutoRoleIcon;
        protected int PermaRoleIcon;

        public AutoRole(ulong guildId) : base(guildId)
        {
            Enabled = true;
            AutoRoleIcon = 128126;
            PermaRoleIcon = 127918;
        }

        public bool Enabled { get; }

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