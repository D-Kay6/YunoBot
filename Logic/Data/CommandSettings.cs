using System;

namespace Logic.Data
{
    [Serializable]
    public class CommandSettings : Configuration<CommandSettings>
    {
        public CommandSettings(ulong guildId) : base(guildId)
        {
            Prefix = "/";
        }

        public string Prefix { get; private set; }

        public bool ChangePrefix(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;
            Prefix = value;
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