using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yuno.Data.Core.Structs.Data
{
    [Serializable]
    public struct StoredData
    {
        public AutoChannelData AutoChannel { get; private set; }
        public CommandSettingsData CommandSettings { get; private set; }

        public StoredData(AutoChannelData autoChannel, CommandSettingsData commandSettings)
        {
            this.AutoChannel = autoChannel;
            this.CommandSettings = commandSettings;
        }
    }
}
