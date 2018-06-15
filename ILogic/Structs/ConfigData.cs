using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILogic.Structs
{
    public struct ConfigData
    {
        public string Prefix;
        public string Token;

        public ConfigData(string prefix, string token)
        {
            this.Prefix = prefix;
            this.Token = token;
        }
    }
}
