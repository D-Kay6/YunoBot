using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yuno.Data.Core.Structs.Data
{
    public struct CommandSettingsData
    {
        public string Prefix { get; private set; }

        public CommandSettingsData(string prefix = "/")
        {
            this.Prefix = prefix;
        }
    }
}
