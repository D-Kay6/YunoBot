using Yuno.Logic.Core;

namespace Yuno.Logic
{
    public class CommandSettingsLogic : CommandSettings
    {
        public bool ChangePrefix(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;
            this.Prefix = value;
            return true;
        }
    }
}
