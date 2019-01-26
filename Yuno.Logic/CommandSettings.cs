using System;

namespace Yuno.Logic
{
    [Serializable]
    public class CommandSettings : Configuration<CommandSettings>
    {
        public string Prefix { get; private set; }

        public CommandSettings(ulong guildId) : base(guildId)
        {
            this.Prefix = "/";
        }

        public bool ChangePrefix(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;
            this.Prefix = value;
            return true;
        }

        public override void Save()
        {
            Save(this);
        }

        protected override void Update()
        {
            //do nothing;
        }
    }
}