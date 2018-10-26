using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yuno.Logic.Core
{
    public class CommandSettings
    {
        public string Prefix { get; protected set; }

        public CommandSettings()
        {
            this.Prefix = "/";
        }
    }
}
